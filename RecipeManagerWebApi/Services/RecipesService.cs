using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Inputs;
using RecipeManagerWebApi.Types.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RecipeManagerWebApi.Utilities.ModelFilterFactory;
using RecipeManagerWebApi.Types.ModelFilter;

namespace RecipeManagerWebApi.Services
{
    public class RecipesService : IRecipesService
    {
        private readonly ILogger<RecipesService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageController _blobStorageController;
        private readonly IValidator<RecipeModel> _recipeValidator;

        public RecipesService(ILogger<RecipesService> logger, IUnitOfWork unitOfWork, IBlobStorageController blobStorageController, IValidator<RecipeModel> recipeValidator)
        {
            _logger = logger;
            _unitOfWork = unitOfWork; 
            _blobStorageController = blobStorageController;
            _recipeValidator = recipeValidator;
        }

        public async Task<Recipe> GetRecipe(string recipeName)
        {
            _logger.LogInformation($"Finding recipe with name '{recipeName}' from the recipesRepository");
            RecipeModel recipeModel = await _unitOfWork.RecipesRepository.Find(recipeName);

            if (recipeModel == null)
            {
                _logger.LogError($"Recipe with name '{recipeName}' was not found in the recipesRepository");
                throw new WebApiException(HttpStatusCode.NotFound);
            }

            return new Recipe(recipeModel, await ResolveRecipeIngredients(recipeModel));
        }

        public async Task<PaginatedResponse<RecipeListItem>> GetAllRecipes(IDictionary<string, List<PropertyFilter>> propertyQueryFilters)
        {
            ModelFilterFactory modelFilterFactory = new ModelFilterFactory();
            RecipeModelFilter recipeModelFilter = modelFilterFactory.CreateRecipeModelFilter(propertyQueryFilters);

            _logger.LogInformation($"Finding recipes from the recipesRepository");
            IEnumerable<RecipeModel> recipeModels = await _unitOfWork.RecipesRepository.FindAll(recipeModelFilter);

            if (recipeModels.Count() == 0)
            {
                _logger.LogInformation("No recipes were found in the recipesRepository");
            }

            _logger.LogInformation("Finding total recipes from the recipesRepository");
            int total = await _unitOfWork.RecipesRepository.GetLength(recipeModelFilter); 

            IEnumerable<RecipeListItem> recipes = recipeModels.Select(recipeModel => new RecipeListItem(recipeModel));

            return new PaginatedResponse<RecipeListItem>(recipes, recipeModelFilter.Offset, total);
        }

        public async Task<Recipe> CreateRecipe(RecipeCreateInput recipeCreateInput, IEnumerable<RecipeIngredientInput> recipeIngredientsInput, IEnumerable<InstructionInput> instructionsInput)
        {
            _logger.LogInformation($"Checking that recipe does not already exist in the recipesRepository");
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Find(recipeCreateInput.RecipeName);
            if (existingRecipeModel != null)
            {
                _logger.LogError($"Recipe with name ${recipeCreateInput.RecipeName} already exists in the recipesRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Recipe with name ${recipeCreateInput.RecipeName} already exists");
            }

            IEnumerable<RecipeIngredientModel> recipeIngredientModels = await ResolveRecipeIngredientModels(recipeIngredientsInput); //Will resolve the recipe inputs to their corresponding models and also validate that they exist as well
            IEnumerable<InstructionModel> instructionModels = instructionsInput.Select(instructionInput => new InstructionModel(instructionInput));

            _logger.LogInformation("Uploading image file to external blob storage container");
            string fileName = $"recipe_{recipeCreateInput.RecipeName}";
            string imageUrl = _blobStorageController.UploadFile(recipeCreateInput.ImageFile, fileName);

            RecipeModel recipeModel = new RecipeModel(recipeCreateInput, imageUrl, recipeIngredientModels, instructionModels); //Creates a recipe model based off of the input and filename since the repository uses models. 

            _logger.LogInformation("Validating recipe model");
            ValidationResult validationResult = _recipeValidator.Validate(recipeModel);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Recipe data illegal");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Not allowed to insert recipe due to illegal data.");
            }

            _logger.LogInformation($"Inserting recipe into the recipesRepository");
            await _unitOfWork.RecipesRepository.Insert(recipeModel);

            _unitOfWork.Commit();

            return new Recipe(recipeModel, await ResolveRecipeIngredients(recipeModel));
        }

        public async Task<Recipe> UpdateRecipe(string recipeName, RecipeUpdateInput recipeUpdateInput)
        {
            _logger.LogInformation($"Checking that recipe exists in the recipesRepository");
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Find(recipeName); 
            if (existingRecipeModel == null)
            {
                _logger.LogError($"Recipe with name ${recipeName} does not exist in the recipesRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Recipe with name ${recipeName} does not exist");
            }

            RecipeModel recipeModel = new RecipeModel(recipeUpdateInput, existingRecipeModel); //Again takes an existing model to fill in any missing data the update input doesn't cover

            _logger.LogInformation("Validating recipe model");
            ValidationResult validationResult = _recipeValidator.Validate(recipeModel);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Recipe data illegal");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Not allowed to update recipe due to illegal data.");
            }

            _logger.LogInformation($"Updating recipe in the recipesRepository");
            await _unitOfWork.RecipesRepository.Update(existingRecipeModel.Id, recipeModel);

            _unitOfWork.Commit();

            return new Recipe(recipeModel, await ResolveRecipeIngredients(recipeModel));
        }

        public async Task<IEnumerable<RecipeIngredient>> UpdateRecipeIngredients(string recipeName, IEnumerable<RecipeIngredientInput> recipeIngredientInputs)
        {
            _logger.LogInformation($"Checking that recipe exists in the recipesRepository");
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Find(recipeName);
            if (existingRecipeModel == null)
            {
                _logger.LogError($"Recipe with name ${recipeName} does not exist in the recipesRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Recipe with name ${recipeName} does not exist");
            }

            IEnumerable<RecipeIngredientModel> recipeIngredientModels = await ResolveRecipeIngredientModels(recipeIngredientInputs);
            existingRecipeModel.Ingredients = recipeIngredientModels;

            _logger.LogInformation("Validating recipe model");
            ValidationResult validationResult = _recipeValidator.Validate(existingRecipeModel);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Recipe ingredient data illegal"); //Even though it validate the entire recipe model, since recipe ingredients are only changed can assume they're the issue
                throw new WebApiException(HttpStatusCode.Forbidden, $"Not allowed to update recipe ingredients due to illegal data.");
            }

            _logger.LogInformation($"Updating recipe in the recipesRepository");
            await _unitOfWork.RecipesRepository.Update(existingRecipeModel.Id, existingRecipeModel);

            _unitOfWork.Commit();

            return await ResolveRecipeIngredients(existingRecipeModel);
        }

        public async Task<IEnumerable<Instruction>> UpdateInstructions(string recipeName, IEnumerable<InstructionInput> instructionInputs)
        {
            _logger.LogInformation($"Checking that recipe exists in the recipesRepository");
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Find(recipeName);
            if (existingRecipeModel == null)
            {
                _logger.LogError($"Recipe with name ${recipeName} does not exist in the recipesRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Recipe with name ${recipeName} does not exist");
            }

            IEnumerable<InstructionModel> instructions = instructionInputs.Select(
                instructionInput => new InstructionModel(instructionInput));
            existingRecipeModel.Instructions = instructions;

            _logger.LogInformation("Validating recipe model");
            ValidationResult validationResult = _recipeValidator.Validate(existingRecipeModel);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Instruction data illegal"); //Even though it validate the entire recipe model, since recipe ingredients are only changed can assume they're the issue
                throw new WebApiException(HttpStatusCode.Forbidden, $"Not allowed to update instructions due to illegal data.");
            }

            _logger.LogInformation($"Updating recipe in the recipesRepository");
            await _unitOfWork.RecipesRepository.Update(existingRecipeModel.Id, existingRecipeModel);

            _unitOfWork.Commit();

            return existingRecipeModel.Instructions.Select(
                instructionModel => new Instruction(instructionModel));
        }

        public async Task<string> UploadRecipeImage(string recipeName, IFormFile imageFile)
        {
            _logger.LogInformation($"Checking that recipe exists in the recipesRepository");
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Find(recipeName);
            if (existingRecipeModel == null)
            {
                _logger.LogError($"Recipe with name ${recipeName} does not exist in the recipesRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Recipe with name ${recipeName} does not exist");
            }

            _logger.LogInformation("Checking that image file exists");
            if (imageFile == null)
            {
                _logger.LogError("Image file does not exist");
                throw new WebApiException(HttpStatusCode.Forbidden, "Image file not given");
            }

            _logger.LogInformation("Uploading image file to external blob storage container");
            string fileName = $"recipe_{recipeName}";
            existingRecipeModel.ImageUrl = _blobStorageController.UploadFile(imageFile, fileName); 

            _logger.LogInformation($"Updating recipe in the recipesRepository");
            await _unitOfWork.RecipesRepository.Update(existingRecipeModel.Id, existingRecipeModel);

            _unitOfWork.Commit();

            return existingRecipeModel.ImageUrl;
        }

        public async Task RemoveRecipeImage(string recipeName)
        {
            _logger.LogInformation($"Checking that recipe exists in the recipesRepository");
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Find(recipeName);
            if (existingRecipeModel == null)
            {
                _logger.LogError($"Recipe with name ${recipeName} does not exist in the recipesRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Recipe with name ${recipeName} does not exist");
            }

            _logger.LogInformation("Searching for image file on external blob storage container and deleting it if found");
            _blobStorageController.DeleteFileIfExists(existingRecipeModel.ImageUrl);
            existingRecipeModel.ImageUrl = null;

            _logger.LogInformation($"Updating recipe in the recipesRepository");
            await _unitOfWork.RecipesRepository.Update(existingRecipeModel.Id, existingRecipeModel);

            _unitOfWork.Commit();
        }

        public async Task DeleteRecipe(string recipeName)
        {
            _logger.LogInformation($"Checking that recipe exists in the recipesRepository");
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Find(recipeName);
            if (existingRecipeModel == null)
            {
                _logger.LogError($"Recipe with name ${recipeName} does not exist in the recipesRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"Recipe with name ${recipeName} does not exist");
            }

            _logger.LogInformation("Searching for image file on external blob storage container and deleting it if found");
            _blobStorageController.DeleteFileIfExists(existingRecipeModel.ImageUrl);

            _logger.LogInformation($"Deleting recipe from the recipesRepository");
            await _unitOfWork.RecipesRepository.Delete(existingRecipeModel.Id); 

            _unitOfWork.Commit();
        }

        private async Task<IEnumerable<RecipeIngredient>> ResolveRecipeIngredients(RecipeModel recipeModel)
        {
            //Since the recipeIngredientModel only has some a reference to its corresponding ingredient, each ingredient must be fetched before each RecipeIngredient object can be created

            _logger.LogInformation($"Finding each recipe ingredient's corresponding ingredient model in the ingredientsRepository");
            IEnumerable<IngredientModel> ingredientModels = await _unitOfWork.IngredientsRepository.FindMany(
                recipeModel.Ingredients.Select(ingredient => ingredient.IngredientId), 
                new List<string>());

            if (ingredientModels.Count() < recipeModel.Ingredients.Count())
            {
                _logger.LogError("Some recipe ingredients could not be found in the ingredientsRepository");
                throw new WebApiException(HttpStatusCode.NotFound); //Even though it could be just one ingredient, I think that's enough to throw an exception as it's corrupted data
            }

            return ingredientModels.Select(ingredientModel =>
            {
                RecipeIngredientModel recipeIngredientModel = recipeModel.Ingredients.Single(recipeIngredientModel =>
                    recipeIngredientModel.IngredientId == ingredientModel.Id);
                return new RecipeIngredient(recipeIngredientModel, ingredientModel);
            });
        }

        private async Task<IEnumerable<RecipeIngredientModel>> ResolveRecipeIngredientModels(IEnumerable<RecipeIngredientInput> recipeIngredientInputs)
        {
            //Since the recipeIngredientModel only has some a reference to its corresponding ingredient, each ingredient must be fetched before each RecipeIngredient object can be created

            _logger.LogInformation($"Finding each recipe ingredient's corresponding ingredient model in the ingredientsRepository");
            IEnumerable<IngredientModel> ingredientModels = await _unitOfWork.IngredientsRepository.FindMany(
                new List<int>(),
                recipeIngredientInputs.Select(recipeIngredientInput => recipeIngredientInput.IngredientName));

            if (ingredientModels.Count() < recipeIngredientInputs.Count())
            {
                _logger.LogError($"{ingredientModels.Where(ingredientModel => ingredientModel == null).Count()} ingredients could not be found in the ingredientsRepository");
                throw new WebApiException(HttpStatusCode.Forbidden, $"{ ingredientModels.Where(ingredientModel => ingredientModel == null).Count() } of the given recipe ingredients have no corresponding ingredient records"); 
            }

            return ingredientModels.Select(ingredientModel => {
                RecipeIngredientInput recipeIngredientInput = recipeIngredientInputs.Single(recipeIngredientInput =>
                    recipeIngredientInput.IngredientName == ingredientModel.IngredientName);
                return new RecipeIngredientModel(recipeIngredientInput, ingredientModel.Id);
            });
        }
    }
}
