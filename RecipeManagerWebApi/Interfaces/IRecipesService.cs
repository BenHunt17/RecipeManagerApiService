using Microsoft.AspNetCore.Http;
using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IRecipesService
    {
        Task<Recipe> GetRecipe(string recipeName);

        Task<PaginatedResponse<RecipeListItem>> GetAllRecipes(IDictionary<string, List<PropertyFilter>> propertyQueryFilters);

        Task<Recipe> CreateRecipe(RecipeCreateInput recipeCreateInput, IEnumerable<RecipeIngredientInput> recipeIngredientsInput, IEnumerable<InstructionInput> instructionsInput);

        Task<Recipe> UpdateRecipe(string recipeName, RecipeUpdateInput recipeUpdateInput);

        Task<IEnumerable<RecipeIngredient>> UpdateRecipeIngredients(string recipeName, IEnumerable<RecipeIngredientInput> recipeIngredientUpdateInputs);

        Task<IEnumerable<Instruction>> UpdateInstructions(string recipeName, IEnumerable<InstructionInput> instructionUpdateInputs);

        Task<string> UploadRecipeImage(string recipeName, IFormFile imageFile);

        Task RemoveRecipeImage(string recipeName);

        Task DeleteRecipe(string recipeName);
    }
}
