using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RecipeSchedulerApiService.Enums;
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
    public class RecipesService : IRecipesService
    {
        //Provides the business logic for the recipes endpoint. Things like ensuring that a recipe being created is valid and image uploading etc.

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageController _blobStorageController;
        private readonly IValidator<RecipeModel> _recipeValidator;

        public RecipesService(IUnitOfWork unitOfWork, IBlobStorageController blobStorageController, IValidator<RecipeModel> recipeValidator)
        {
            //Injects the unit of work instance so that the service has free reign over the repositories
            _unitOfWork = unitOfWork;
            _blobStorageController = blobStorageController;
            _recipeValidator = recipeValidator;
        }

        public async Task<RecipeModel> GetRecipe(int id)
        {
            RecipeModel recipeModel = await _unitOfWork.RecipesRepository.Get(id);

            foreach (RecipeIngredientModel recipeIngredientModel in recipeModel.Ingredients)
            {
                //Goes through each recipe ingredient and scales their stats according to the quantity. This needs to be done because the stats are ripped from the ingredient at default value in the database
                recipeIngredientModel.ScaleRecipeIngredientStatistics();
            }

            return recipeModel;
        }

        public async Task<IEnumerable<RecipeListItem>> GetAllRecipes()
        {
            IEnumerable<RecipeModel> recipeModels = await _unitOfWork.RecipesRepository.GetAll();

            IEnumerable<RecipeListItem> recipes = recipeModels.ToList().Select(recipeModel => new RecipeListItem(recipeModel));

            return recipes;
        }

        public async Task<RecipeModel> CreateRecipe(RecipeCreateInput recipeCreateInput)
        {
            //TODO - These service methods are getting very chunky. May be an idea to seperate sdifferent parts out??

            IEnumerable<RecipeIngredientInput> recipeIngredientsInput = JsonConvert.DeserializeObject<IEnumerable<RecipeIngredientInput>>(recipeCreateInput.RecipeIngredients);
            IEnumerable<InstructionInput> instructionsInput = JsonConvert.DeserializeObject<IEnumerable<InstructionInput>>(recipeCreateInput.Instructions);

            bool recipeNameExists = (await _unitOfWork.RecipesRepository.GetAll()).ToList().Any(recipe => recipe.RecipeName.ToLower() == (recipeCreateInput.RecipeName ?? "").ToLower());

            IEnumerable<int> existingIngredientIds = (await _unitOfWork.IngredientsRepository.GetAll()).ToList().Select(ingredient => ingredient.Id);

            bool IngredientsListContainsNonExistentIngredient = recipeIngredientsInput.Any(recipeIngredient => !existingIngredientIds.ToList().Contains(recipeIngredient.IngredientId)); //Checks to make sure that every ingredient Id in the input references a real ingredient in the database. Will really need to think about this way of doing things cause I don't like querying every entry just to validate something

            if (recipeNameExists || IngredientsListContainsNonExistentIngredient)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            string fileName = $"recipe_{recipeCreateInput.RecipeName}";

            string imageUrl = "";
            if(recipeCreateInput.ImageFile == null)
            {
                imageUrl = null;
            }
            else
            {
                imageUrl = _blobStorageController.GetUrlByFileName(fileName);
            }

            foreach (RecipeIngredientInput recipeIngredientInput in recipeIngredientsInput)
            {
                //Loops through each recipe ingredient and standardises its quantity before adding to the database

                MeasureType measureType = EnumUtilities.StringToMeasureType(
                    (await _unitOfWork.IngredientsRepository.Get(recipeIngredientInput.IngredientId)).MeasureTypeValue);

                recipeIngredientInput.Quantity = IngredientUtilities.StandardiseIngredientQuantity(
                    recipeIngredientInput.Quantity, measureType);
            }

            RecipeModel recipeModel = new RecipeModel(recipeCreateInput, imageUrl, recipeIngredientsInput, instructionsInput); //Creates a recipe model based off of the input and filename since the repository uses models. 

            ValidationResult validationResult = _recipeValidator.Validate(recipeModel);

            if (!validationResult.IsValid)
            {
                _blobStorageController.DeleteFileIfExists(fileName); 

                throw new ValidationException(validationResult.Errors);
            }

            int entryId = await _unitOfWork.RecipesRepository.Add(recipeModel);

            if (entryId < 0)
            {
                _unitOfWork.RollBack();

                _blobStorageController.DeleteFileIfExists(fileName); 

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //This is very important especially for repository methods like the add recipe. Since there are loads of database calls and lots of granualar data entries being added, if one of them were to fail halfway through, then the transaction could be rolled
            //back and the data which was added before wouldn't persist since this commit would never be reached.
            _unitOfWork.Commit();

            _blobStorageController.UploadFile(recipeCreateInput.ImageFile, fileName); //If the commit is successful then it is "safe" for the new image to be uploaded too

            RecipeModel newRecipeModel = await GetRecipe(entryId); //Fetches newly created recipe. Uses exisitng method so the scaling is done already. 

            return newRecipeModel;
        }

        public async Task<RecipeModel> UpdateRecipe(int id, RecipeUpdateInput recipeUpdateInput)
        {
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Get(id);

            if (existingRecipeModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            RecipeModel recipeModel = new RecipeModel(recipeUpdateInput);
            recipeModel.ImageUrl = existingRecipeModel.ImageUrl;
            recipeModel.Ingredients = existingRecipeModel.Ingredients;
            recipeModel.Instructions = existingRecipeModel.Instructions;

            ValidationResult validationResult = _recipeValidator.Validate(recipeModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.RecipesRepository.Update(id, recipeModel);

            _unitOfWork.Commit();

            RecipeModel newRecipeModel = await GetRecipe(id); //Fetches newly created recipe. Uses exisitng method so the scaling is done already. 

            return newRecipeModel;
        }

        public async Task<RecipeModel> UpdateRecipeIngredients(int id, IEnumerable<RecipeIngredientInput> recipeIngredientInputs)
        {
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Get(id);

            if (existingRecipeModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            foreach (RecipeIngredientInput recipeIngredientInput in recipeIngredientInputs)
            {
                //Loops through each recipe ingredient and standardises its quantity before adding to the database

                //TODO - maybe seperate this into private method
                MeasureType measureType = EnumUtilities.StringToMeasureType(
                    (await _unitOfWork.IngredientsRepository.Get(recipeIngredientInput.IngredientId)).MeasureTypeValue);

                recipeIngredientInput.Quantity = IngredientUtilities.StandardiseIngredientQuantity(
                    recipeIngredientInput.Quantity, measureType);
            }

            IEnumerable<RecipeIngredientModel> updatedRecipeIngredients = recipeIngredientInputs.Select(ingredient => new RecipeIngredientModel(ingredient)); //Maps each ingredient input to an ingredient modal
            existingRecipeModel.Ingredients = updatedRecipeIngredients; //Rest of the model stays the same except the ingredients which are reassigned

            ValidationResult validationResult = _recipeValidator.Validate(existingRecipeModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.RecipesRepository.Update(id, existingRecipeModel);

            _unitOfWork.Commit();

            RecipeModel newRecipeModel = await GetRecipe(id); 

            return newRecipeModel;
        }

        public async Task<RecipeModel> UpdateInstructions(int id, IEnumerable<InstructionInput> instructionInputs)
        {
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Get(id);

            if (existingRecipeModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            IEnumerable<InstructionModel> updatedRecipeInstructions = instructionInputs.Select(instruction => new InstructionModel(instruction)); 
            existingRecipeModel.Instructions = updatedRecipeInstructions;

            ValidationResult validationResult = _recipeValidator.Validate(existingRecipeModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.RecipesRepository.Update(id, existingRecipeModel);

            _unitOfWork.Commit();

            RecipeModel newRecipeModel = await GetRecipe(id);

            return newRecipeModel;
        }

        public async Task<RecipeModel> UploadRecipeImage(int id, IFormFile imageFile)
        {
            if (imageFile == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Get(id); 

            if (existingRecipeModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            string fileName = $"recipe{existingRecipeModel.RecipeName}";
            string imageUrl = _blobStorageController.GetUrlByFileName(fileName);

            existingRecipeModel.ImageUrl = imageUrl;

            await _unitOfWork.RecipesRepository.Update(id, existingRecipeModel);

            _unitOfWork.Commit();

            _blobStorageController.UploadFile(imageFile, fileName);

            RecipeModel newRecipeModel = await GetRecipe(id);

            return newRecipeModel;
        }

        public async Task<RecipeModel> RemoveRecipeImage(int id)
        {
            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Get(id);

            if (existingRecipeModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            existingRecipeModel.ImageUrl = null; 

            await _unitOfWork.RecipesRepository.Update(id, existingRecipeModel);

            _unitOfWork.Commit();

            string fileName = $"recipe_{existingRecipeModel.RecipeName}";
            _blobStorageController.DeleteFileIfExists(fileName);

            RecipeModel newRecipeModel = await GetRecipe(id);

            return newRecipeModel;
        }

        public async Task<RecipeModel> DeleteRecipe(int id)
        {
            //Removes recipe with a certain ID and its data from other tables all from the database

            RecipeModel existingRecipeModel = await _unitOfWork.RecipesRepository.Get(id);

            if (existingRecipeModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            await _unitOfWork.RecipesRepository.Delete(id); 

            string fileName = $"recipe_{existingRecipeModel.RecipeName}";

            _blobStorageController.DeleteFileIfExists(fileName); 

            _unitOfWork.Commit();

            return existingRecipeModel;
        }
    }
}
