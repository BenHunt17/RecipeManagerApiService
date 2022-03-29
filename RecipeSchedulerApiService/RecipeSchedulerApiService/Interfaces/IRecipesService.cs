using RecipeSchedulerApiService.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IRecipesService
    {
        Task<Recipe> GetRecipe(int id);

        Task<IEnumerable<RecipeListItem>> GetAllRecipes();
    }
}
