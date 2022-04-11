using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types;
using RecipeSchedulerApiService.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Services
{
    public class RecipesService : IRecipesService
    {
        //Provides the business logic for the recipes endpoint. Things like ensuring that a recipe being created is valid and image uploading etc.

        private readonly IUnitOfWork _unitOfWork;

        public RecipesService(IUnitOfWork unitOfWork)
        {
            //Injects the unit of work instance so that the service has free reign over the repositories
            _unitOfWork = unitOfWork;
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
    }
}
