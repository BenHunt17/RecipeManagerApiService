using FluentValidation;
using FluentValidation.Results;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types;
using RecipeSchedulerApiService.Types.Inputs;
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
        private readonly IAzureBlobStorageController _azureBlobStorageController;
        private readonly IValidator<IngredientModel> _ingredientValidator;

        public IngredientsService(IUnitOfWork unitOfWork, IAzureBlobStorageController azureBlobStorageController, IValidator<IngredientModel> ingredientValidator)
        {
            _unitOfWork = unitOfWork;
            _azureBlobStorageController = azureBlobStorageController;
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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            string fileName = $"ingredient_{ingredientCreateInput.IngredientName}";

            string imageUrl = _azureBlobStorageController.UploadFile(ingredientCreateInput.ImageFile, fileName); //Will return null if image file was null

            IngredientModel ingredientModel = new IngredientModel(ingredientCreateInput, imageUrl);

            ValidationResult validationResult = _ingredientValidator.Validate(ingredientModel);

            if (!validationResult.IsValid)
            {
                _azureBlobStorageController.DeleteFileIfExists(fileName); //Deletes the image from blob storage if it was added since the ingredient model isn't valid

                throw new ValidationException(validationResult.Errors);
            }

            int entryId = await _unitOfWork.IngredientsRepository.Add(ingredientModel);

            if (entryId < 0)
            {
                //If the entryId isn't a valid index, then it is assumed the create failed and so any work done to the database is rolled back
                _unitOfWork.RollBack();

                _azureBlobStorageController.DeleteFileIfExists(fileName); //Deletes the image from blob storage if it was added since the ingredient wasn't successfully added to the database

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            _unitOfWork.Commit(); //If the gaurd clauses are bypassed then it is assumed everything worked and the changes are commited

            ingredientModel.Id = entryId; //Assigns the returned entry Id to the Id field

            return ingredientModel;
        }
    }
}
