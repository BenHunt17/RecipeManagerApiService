using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IRecipesService
    {
        Task<RecipeModel> GetRecipe(int id);

        Task<IEnumerable<RecipeListItem>> GetAllRecipes();
    }
}
