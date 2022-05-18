using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types;
using RecipeSchedulerApiService.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IIngredientsService
    {
        Task<IngredientModel> GetIngredient(int id);

        Task<IEnumerable<IngredientListItem>> GetAllIngredients();

        Task<IngredientModel> CreateIngredient(IngredientCreateInput ingredientCreateInput);

        Task<IngredientModel> UpdateIngredient(int id, IngredientUpdateInput ingredientUpdateInput);

        Task<IngredientModel> DeleteIngredient(int id);
    }
}
