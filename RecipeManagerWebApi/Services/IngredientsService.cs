using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types;
using RecipeManagerWebApi.Types.Inputs;
using RecipeManagerWebApi.Types.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RecipeManagerWebApi.Repositories.ModelSearch;

namespace RecipeManagerWebApi.Services
{
    public class IngredientsService : IIngredientsService
    {
        //Provides business logic for ingredients. Note this service is specifically for ingredients themselves and not at a recipe level

        private readonly ILogger<IngredientsService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageController _blobStorageController;
        private readonly IValidator<IngredientModel> _ingredientValidator;

        public IngredientsService(ILogger<IngredientsService> logger, IUnitOfWork unitOfWork, IBlobStorageController blobStorageController, IValidator<IngredientModel> ingredientValidator)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _blobStorageController = blobStorageController;
            _ingredientValidator = ingredientValidator;
        }

        public async Task<Ingredient> GetIngredient(string ingredientName)
        {
            _logger.LogInformation($"Finding ingredient with name '{ingredientName}' from the ingredientsRepository");
            IngredientModel ingredientModel = await _unitOfWork.IngredientsRepository.Find(ingredientName);

            if (ingredientModel == null)
            {
                _logger.LogError($"Ingredient with name '{ingredientName}' was not found in the ingredientsRepository");
                throw new WebApiException(HttpStatusCode.NotFound); //Since not found status code doesn't allow anything to be returned, don't specify message
            }

            return new Ingredient(ingredientModel);
        }

        public async Task<IEnumerable<IngredientListItem>> GetIngredients()
        {
            //TODO - This should soon take filter and pagination arguments

            DataSearch<IngredientModelFilter> dataSearch = new DataSearch<IngredientModelFilter>();

            _logger.LogInformation($"Finding ingredients from the ingredientsRepository");
            IEnumerable<IngredientModel> ingredientModels = await _unitOfWork.IngredientsRepository.FindAll(dataSearch);

            if (ingredientModels.Count() == 0)
            {
                _logger.LogInformation("No ingredients were found in the ingredientsRepository");
            }

            return ingredientModels.Select(ingredient => new IngredientListItem(ingredient));
        }

        public async Task<Ingredient> CreateIngredient(IngredientCreateInput ingredientCreateInput)
        {
            _logger.LogInformation($"Checking that ingredient does not already exist in the ingredientsRepository");
            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Find(ingredientCreateInput.IngredientName);
            if (existingIngredientModel != null)
            {
                //If the ingredient model already exists then throw an exception before any damage can be done
                _logger.LogError($"Ingredient with name ${ingredientCreateInput.IngredientName} already exists in the ingredientsRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Ingredient with name ${ingredientCreateInput.IngredientName} already exists");
            }

            _logger.LogInformation("Uploading image file to external blob storage container");
            string fileName = $"ingredient_{ingredientCreateInput.IngredientName}";
            string imageUrl = _blobStorageController.UploadFile(ingredientCreateInput.ImageFile, fileName);

            IngredientModel ingredientModel = new IngredientModel(ingredientCreateInput, imageUrl);

            _logger.LogInformation("Validating ingredient model");
            ValidationResult validationResult = _ingredientValidator.Validate(ingredientModel);
            if (!validationResult.IsValid)
            {
                //Don't expose actual validtion errors as model details should be hidden. 
                //TODO - Maybe there should be input validators, maybe in controllers?
                //TODO - Log the errors
                _logger.LogError($"Ingredient data illegal");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Not allowed to insert ingredient due to illegal data."); 
            }

            _logger.LogInformation($"Inserting ingredient into the ingredientsRepository");
            await _unitOfWork.IngredientsRepository.Insert(ingredientModel);

            _unitOfWork.Commit(); 

            return new Ingredient(ingredientModel);
        }
        public async Task<Ingredient> UpdateIngredient(string ingredientName, IngredientUpdateInput ingredientUpdateInput)
        {
            _logger.LogInformation($"Checking that ingredient exists in the ingredientsRepository");
            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Find(ingredientName); 
            if (existingIngredientModel == null)
            {
                _logger.LogError($"Ingredient with name ${ingredientName} does not exist in the ingredientsRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Ingredient with name ${ingredientName} does not exist");
            }

            IngredientModel ingredientModel = new IngredientModel(ingredientUpdateInput, existingIngredientModel); //The ingredient model's update constructor takes existing ingredient model to fill in the gaps (imageUrl)

            _logger.LogInformation("Validating ingredient model");
            ValidationResult validationResult = _ingredientValidator.Validate(ingredientModel);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Ingredient data illegal");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Not allowed to update ingredient due to illegal data.");
            }

            _logger.LogInformation($"Updating ingredient in the ingredientsRepository");
            await _unitOfWork.IngredientsRepository.Update(existingIngredientModel.Id, ingredientModel); 

            _unitOfWork.Commit();

            return new Ingredient(ingredientModel);
        }

        public async Task<string> UploadIngredientImage(string ingredientName, IFormFile imageFile)
        {
            _logger.LogInformation($"Checking that ingredient exists in the ingredientsRepository");
            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Find(ingredientName);
            if (existingIngredientModel == null)
            {
                _logger.LogError($"Ingredient with name ${ingredientName} does not exist in the ingredientsRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Ingredient with name ${ingredientName} does not exist");
            }

            _logger.LogInformation("Checking that image file exists");
            if(imageFile == null)
            {
                _logger.LogError("Image file does not exist");
                throw new WebApiException(HttpStatusCode.Forbidden, "Image file not given");
            }

            _logger.LogInformation("Uploading image file to external blob storage container");
            string fileName = $"ingredient_{ingredientName}";
            existingIngredientModel.ImageUrl = _blobStorageController.UploadFile(imageFile, fileName); //When an update to a model is as simple as a field assignment, then just use the exisitng model instead of making another cluttered constructor in the model class

            //Since the model comes from the database and the only field being updated is the imageUrl which is coming from the blob storage controller, it can safely be assumed that this model
            //is safe and doesn't need validating
            _logger.LogInformation($"Updating ingredient in the ingredientsRepository");
            await _unitOfWork.IngredientsRepository.Update(existingIngredientModel.Id, existingIngredientModel);

            _unitOfWork.Commit();

            return existingIngredientModel.ImageUrl;
        }

        public async Task RemoveIngredientImage(string ingredientName)
        {
            _logger.LogInformation($"Checking that ingredient exists in the ingredientsRepository");
            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Find(ingredientName);
            if (existingIngredientModel == null)
            {
                _logger.LogError($"Ingredient with name ${ingredientName} does not exist in the ingredientsRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Ingredient with name ${ingredientName} does not exist");
            }

            _logger.LogInformation("Searching for image file on external blob storage container and deleting it if found");
            _blobStorageController.DeleteFileIfExists(existingIngredientModel.ImageUrl);
            existingIngredientModel.ImageUrl = null;

            _logger.LogInformation($"Updating ingredient in the ingredientsRepository");
            await _unitOfWork.IngredientsRepository.Update(existingIngredientModel.Id, existingIngredientModel);

            _unitOfWork.Commit();
        }

        public async Task DeleteIngredient(string ingredientName)
        {
            _logger.LogInformation($"Checking that ingredient exists in the ingredientsRepository");
            IngredientModel existingIngredientModel = await _unitOfWork.IngredientsRepository.Find(ingredientName);
            if (existingIngredientModel == null)
            {
                _logger.LogError($"Ingredient with name ${ingredientName} does not exist in the ingredientsRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Ingredient with name ${ingredientName} does not exist");
            }

            _logger.LogInformation("Searching for image file on external blob storage container and deleting it if found");
            _blobStorageController.DeleteFileIfExists(existingIngredientModel.ImageUrl);

            _logger.LogInformation($"Deleting ingredient from the ingredientsRepository");
            await _unitOfWork.IngredientsRepository.Delete(existingIngredientModel.Id); 

            _unitOfWork.Commit(); 
        }
    }
}
