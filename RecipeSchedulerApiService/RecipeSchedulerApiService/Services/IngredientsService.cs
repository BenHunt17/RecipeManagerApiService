using FluentValidation;
using FluentValidation.Results;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types;
using RecipeSchedulerApiService.Types.Inputs;
using RecipeSchedulerApiService.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace RecipeSchedulerApiService.Services
{
    public class IngredientsService : IIngredientsService
    {
        //Provides business logic for ingredients. Note this service is specifically for ingredients themselves and not at a recipe level

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageController _blobStorageController;
        private readonly IValidator<IngredientModel> _ingredientValidator;

        public IngredientsService(IUnitOfWork unitOfWork, IBlobStorageController blobStorageController, IValidator<IngredientModel> ingredientValidator)
        {
            _unitOfWork = unitOfWork;
            _blobStorageController = blobStorageController;
            _ingredientValidator = ingredientValidator;
        }

        public async Task<IngredientModel> GetIngredient(int id)
        {
            IngredientModel ingredientModel = await _unitOfWork.IngredientsRepository.Get(id);

            return ingredientModel;
        }

        public async Task<IEnumerable<IngredientListItem>> GetAllIngredients()
        {
            IEnumerable<IngredientModel> ingredientModels = await _unitOfWork.IngredientsRepository.GetAll();

            IEnumerable<IngredientListItem> ingredients = ingredientModels.ToList().Select(ingredientModel => new IngredientListItem(ingredientModel));

            return ingredients;
        }

        public async Task<IngredientModel> CreateIngredient(IngredientCreateInput ingredientCreateInput)
        {
            bool ingredientNameExists = (await _unitOfWork.IngredientsRepository.GetAll()).ToList().Any(ingredient => ingredient.IngredientName.ToLower() == (ingredientCreateInput.IngredientName ?? "").ToLower()); //TODO: Works for now but will need to investigate a more efficeint method of checking this

            if (ingredientNameExists)
            {
                //If the ingredient name exists then throw an exception before any damage can be done
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            string fileName = $"ingredient_{ingredientCreateInput.IngredientName}";

            string imageUrl = _blobStorageController.UploadFile(ingredientCreateInput.ImageFile, fileName); //Will return null if image file was null

            IngredientModel ingredientModel = new IngredientModel(ingredientCreateInput, imageUrl);

            IngredientUtilities.StandardiseIngredientStatistics(ingredientModel, ingredientCreateInput.Quantity);

            ValidationResult validationResult = _ingredientValidator.Validate(ingredientModel);

            if (!validationResult.IsValid)
            {
                _blobStorageController.DeleteFileIfExists(fileName); //Deletes the image from blob storage if it was added since the ingredient model isn't valid

                throw new ValidationException(validationResult.Errors);
            }

            int entryId = await _unitOfWork.IngredientsRepository.Add(ingredientModel);

            if (entryId < 0)
            {
                //If the entryId isn't a valid index, then it is assumed the create failed and so any work done to the database is rolled back
                _unitOfWork.RollBack();

                _blobStorageController.DeleteFileIfExists(fileName); //Deletes the image from blob storage if it was added since the ingredient wasn't successfully added to the database

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            _unitOfWork.Commit(); //If the gaurd clauses are bypassed then it is assumed everything worked and the changes are commited

            ingredientModel.Id = entryId; //Assigns the returned entry Id to the Id field

            return ingredientModel;
        }
        public async Task<IngredientModel> UpdateIngredient(int id, IngredientUpdateInput ingredientUpdateInput)
        {
            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Get(id);

            if (existingIngredientModel == null)
            {
                //Throws an error if there isn't an existing recipe model with the id
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            IngredientModel ingredientModel = new IngredientModel(ingredientUpdateInput);

            ValidationResult validationResult = _ingredientValidator.Validate(ingredientModel);

            if (!validationResult.IsValid)
            {
                //Returns an error if the recipe model is not valid.
                throw new ValidationException(validationResult.Errors);
            }

            ingredientModel.ImageUrl = existingIngredientModel.ImageUrl; //Assign the existing url since this update method doesn't take image url in the input

            IngredientUtilities.StandardiseIngredientStatistics(ingredientModel, ingredientUpdateInput.Quantity); //Standardises the statistics for the ingredient model before updating the repository

            await _unitOfWork.IngredientsRepository.Update(id, ingredientModel); //Updates the repository and waits for it to complete

            ingredientModel = await _unitOfWork.IngredientsRepository.Get(id); //Fetches the new entry so that the entire model can be returned.

            _unitOfWork.Commit();

            return ingredientModel;
        }

        public async Task<IngredientModel> DeleteIngredient(int id)
        {
            //Removes an ingredient with a certain ID from the database

            IngredientModel ingredientModel = await _unitOfWork.IngredientsRepository.Get(id);

            if(ingredientModel == null)
            {
                //If ingredient doesn't exist then throw an exception
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
                
            await _unitOfWork.IngredientsRepository.Delete(id); //Wait for repository remove to finish in case an error occurs

            string fileName = $"ingredient_{ingredientModel.IngredientName}";

            _blobStorageController.DeleteFileIfExists(fileName); //Should delete the image from blob storage as well

            _unitOfWork.Commit(); //Commits the deletion of the ingredient

            return ingredientModel; //Returns the ingredient model of the deleted ingredient so that the consumer has as much info as possible
        }
    }
}
