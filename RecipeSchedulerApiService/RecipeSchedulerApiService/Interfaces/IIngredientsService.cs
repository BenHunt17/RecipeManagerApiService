using RecipeSchedulerApiService.Types;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IIngredientsService
    {
        Task<Ingredient> GetIngredient(int id);
    }
}
