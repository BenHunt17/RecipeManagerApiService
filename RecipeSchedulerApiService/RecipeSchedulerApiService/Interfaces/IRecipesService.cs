using Microsoft.AspNetCore.Http;
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

        Task<RecipeModel> UpdateRecipe(int id, RecipeUpdateInput recipeUpdateInput);

        Task<RecipeModel> UpdateRecipeIngredients(int id, IEnumerable<RecipeIngredientInput> recipeIngredientUpdateInputs);

        Task<RecipeModel> UpdateInstructions(int id, IEnumerable<InstructionInput> instructionUpdateInputs);

        Task<RecipeModel> UploadRecipeImage(int id, IFormFile imageFile);

        Task<RecipeModel> RemoveRecipeImage(int id);

        Task<RecipeModel> DeleteRecipe(int id);
    }
}
