using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types;
using RecipeSchedulerApiService.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IRecipesService
    {
        Task<RecipeModel> GetRecipe(int id);

        Task<IEnumerable<RecipeListItem>> GetAllRecipes();

        Task<RecipeModel> CreateRecipe(RecipeCreateInput recipeCreateInput);

        Task<RecipeModel> DeleteRecipe(int id);
    }
}
