using Microsoft.AspNetCore.Http;
using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Types;
using RecipeManagerWebApi.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManagerWebApi.Types.Common;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IIngredientsService
    {
        Task<Ingredient> GetIngredient(string ingredientName);

        Task<IEnumerable<IngredientListItem>> GetIngredients(IDictionary<string, List<PropertyFilter>> propertyQueryFilters); //TODO - Maybe put this ugly dictionary type behind a wrapper class just so that there's something nicer to look at 

        Task<Ingredient> CreateIngredient(IngredientCreateInput ingredientCreateInput);

        Task<Ingredient> UpdateIngredient(string ingredientName, IngredientUpdateInput ingredientUpdateInput);

        Task<string> UploadIngredientImage(string ingredientName, IFormFile imageFile);

        Task RemoveIngredientImage(string IngredientName);

        Task DeleteIngredient(string ingredientName);
    }
}
