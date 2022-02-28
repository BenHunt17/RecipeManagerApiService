using RecipeSchedulerApiService.Models;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IRecipesService
    {
        Task<RecipeModel> GetRecipe(int id);
    }
}
