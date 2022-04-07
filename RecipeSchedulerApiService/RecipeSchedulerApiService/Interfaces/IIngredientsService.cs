using Microsoft.AspNetCore.Http;
using RecipeSchedulerApiService.Types;
using RecipeSchedulerApiService.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IIngredientsService
    {
        Task<Ingredient> GetIngredient(int id);

        Task<IEnumerable<IngredientListItem>> GetAllIngredients();

        Task<Ingredient> CreateIngredient(IngredientCreateInput ingredientCreateInput);
    }
}
