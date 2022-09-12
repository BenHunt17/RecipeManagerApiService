using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.ModelFilter;
using RecipeManagerWebApi.Types.Models;
using RecipeManagerWebApi.Utilities.ModelFilterFactory.Mappers;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Utilities.ModelFilterFactory
{
    public class ModelFilterFactory
    {
        //Use a simple factory class for mapping filter objects to model filter objects since the code is quite verbose and I don't want it polluting the class constructors

        public IngredientModelFilter CreateIngredientModelFilter(IDictionary<string, List<PropertyFilter>> propertyQueryFilters)
        {
            ModelFilter modelFilter = propertyQueryFilters.MapToModelFilter();
            IngredientModelFilter ingredientModelFilter = propertyQueryFilters.MapToIngredientModelFilter(modelFilter);

			return ingredientModelFilter;
		}

        public RecipeModelFilter CreateRecipeModelFilter(IDictionary<string, List<PropertyFilter>> propertyQueryFilters)
        {
            ModelFilter modelFilter = propertyQueryFilters.MapToModelFilter();
            RecipeModelFilter recipeModelFilter = propertyQueryFilters.MapToRecipeModelFilter(modelFilter);

            return recipeModelFilter;
        }

        public UserModelFilter CreateUserModelFilter(IDictionary<string, List<PropertyFilter>> propertyQueryFilters)
        {
            ModelFilter modelFilter = propertyQueryFilters.MapToModelFilter();
            UserModelFilter userModelFilter = propertyQueryFilters.MapToUserModelFilter(modelFilter);

            return userModelFilter;
        }
    }
}