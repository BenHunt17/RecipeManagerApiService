using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
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

            recipeModel.QuantifyIngredients(); //Scales each of the ingredient's stats to reflect that used in the recipe

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

            bool IngredientsListContainsNonExistentIngredient = recipeIngredientsInput.Any(recipeIngredient => !existingIngredientIds.ToList().Contains(recipeIngredient.RecipeIngredientId)); //Checks to make sure that every ingredient Id in the input references a real ingredient in the database. Will really need to think about this way of doing things cause I don't like querying every entry just to validate something

            if (recipeNameExists || IngredientsListContainsNonExistentIngredient)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            string fileName = $"recipe_{recipeCreateInput.RecipeName}";

            string imageUrl = _blobStorageController.UploadFile(recipeCreateInput.ImageFile, fileName); 

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

            recipeModel = await _unitOfWork.RecipesRepository.Get(entryId); //The recipe model passed into the repository was incomplete so it is fetched using the entryId value returned from the add.

            //This is very important specially for repository methods like the add recipe. Since there are loads of database calls and lots of granualar data entries being added, if one of them were to fail halfway through, then the transaction could be rolled
            //back and the data which was added before wouldn't persist since this commit would never be reached.
            _unitOfWork.Commit();               

            return recipeModel;
        }
    }
}
