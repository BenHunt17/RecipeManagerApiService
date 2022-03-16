using RecipeSchedulerApiService.Types;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IRecipesService
    {
        Task<Recipe> GetRecipe(int id);
    }
}
