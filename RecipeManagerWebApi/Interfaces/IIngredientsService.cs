using Microsoft.AspNetCore.Http;
using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IIngredientsService
    {
        Task<Ingredient> GetIngredient(string ingredientName);

        Task<PaginatedResponse<IngredientListItem>> GetIngredients(IDictionary<string, List<PropertyFilter>> propertyQueryFilters); 

        Task<Ingredient> CreateIngredient(IngredientCreateInput ingredientCreateInput);

        Task<Ingredient> UpdateIngredient(string ingredientName, IngredientUpdateInput ingredientUpdateInput);

        Task<string> UploadIngredientImage(string ingredientName, IFormFile imageFile);

        Task RemoveIngredientImage(string IngredientName);

        Task DeleteIngredient(string ingredientName);
    }
}
