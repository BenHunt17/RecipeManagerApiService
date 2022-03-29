using RecipeSchedulerApiService.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IIngredientsService
    {
        Task<Ingredient> GetIngredient(int id);

        Task<IEnumerable<IngredientListItem>> GetAllIngredients();
    }
}
