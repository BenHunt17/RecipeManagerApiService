using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
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
    //TODO - refactor image upload/delete in creation endpoints since because the new getUrl method was added

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

            string imageUrl = "";
            if(ingredientCreateInput.ImageFile == null)
            {
                imageUrl = null;
            }
            else
            {
                imageUrl = _blobStorageController.GetUrlByFileName(fileName);
            }

            IngredientModel ingredientModel = new IngredientModel(ingredientCreateInput, imageUrl);

            IngredientUtilities.StandardiseIngredientStatistics(ingredientModel, ingredientCreateInput.Quantity);

            ValidationResult validationResult = _ingredientValidator.Validate(ingredientModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            int entryId = await _unitOfWork.IngredientsRepository.Add(ingredientModel);

            if (entryId < 0)
            {
                //If the entryId isn't a valid index, then it is assumed the create failed and so any work done to the database is rolled back
                _unitOfWork.RollBack();

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            _unitOfWork.Commit(); //If the gaurd clauses are bypassed then it is assumed everything worked and the changes are commited

            _blobStorageController.UploadFile(ingredientCreateInput.ImageFile, fileName);

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
            ingredientModel.ImageUrl = existingIngredientModel.ImageUrl; //Assign the existing url since this update method doesn't take image url in the input

            ValidationResult validationResult = _ingredientValidator.Validate(ingredientModel);

            if (!validationResult.IsValid)
            {
                //Returns an error if the recipe model is not valid.
                throw new ValidationException(validationResult.Errors);
            }

            IngredientUtilities.StandardiseIngredientStatistics(ingredientModel, ingredientUpdateInput.Quantity); //Standardises the statistics for the ingredient model before updating the repository

            await _unitOfWork.IngredientsRepository.Update(id, ingredientModel); //Updates the repository and waits for it to complete

            _unitOfWork.Commit();

            IngredientModel newIngredientModel = await GetIngredient(id);

            return newIngredientModel;
        }

        public async Task<IngredientModel> UploadIngredientImage(int id, IFormFile imageFile)
        {
            if(imageFile == null)
            {
                //Only proceed if the image file isn't null
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Get(id); //Fetches the ingredient both to check it exists and because the rest of the data needs to be passed into the repository update

            if (existingIngredientModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            string fileName = $"ingredient_{existingIngredientModel.IngredientName}";
            string imageUrl = _blobStorageController.GetUrlByFileName(fileName); //Gets the container URL so that the database can be updated

            existingIngredientModel.ImageUrl = imageUrl;

            await _unitOfWork.IngredientsRepository.Update(id, existingIngredientModel);

            _unitOfWork.Commit();

            //If this point is reached then the database commit was successful. Therefore it is "safe" for the image to be uploaded overriding any existing image or creating a new one
            _blobStorageController.UploadFile(imageFile, fileName);

            IngredientModel newIngredientModel = await GetIngredient(id);

            return newIngredientModel;
        }

        public async Task<IngredientModel> RemoveIngredientImage(int id)
        {
            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Get(id); 

            if (existingIngredientModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            existingIngredientModel.ImageUrl = null; //Directly sets image url to null since it is going to no longer have one

            await _unitOfWork.IngredientsRepository.Update(id, existingIngredientModel);

            _unitOfWork.Commit();

            //If this point is reached then the database commit was successful. Therefore the image should be removed if it exists
            string fileName = $"ingredient_{existingIngredientModel.IngredientName}";
            _blobStorageController.DeleteFileIfExists(fileName);

            IngredientModel newIngredientModel = await GetIngredient(id);

            return newIngredientModel;
        }

        public async Task<IngredientModel> DeleteIngredient(int id)
        {
            //Removes an ingredient with a certain ID from the database

            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Get(id);

            if(existingIngredientModel == null)
            {
                //If ingredient doesn't exist then throw an exception
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
                
            await _unitOfWork.IngredientsRepository.Delete(id); //Wait for repository remove to finish in case an error occurs

            string fileName = $"ingredient_{existingIngredientModel.IngredientName}";

            _blobStorageController.DeleteFileIfExists(fileName); //Should delete the image from blob storage as well

            _unitOfWork.Commit(); //Commits the deletion of the ingredient

            return existingIngredientModel;
        }
    }
}
