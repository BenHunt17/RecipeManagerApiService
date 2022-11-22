using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.ModelFilter;
using RecipeManagerWebApi.Utilities.PropertyFilterExtractor;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Utilities.ModelFilterFactory.Mappers
{
    public static class IngredientModelFilterMapper
    {
        //Another layer of abstraction so that the factory class isn't polluted with this verbose code

        public static IngredientModelFilter MapToIngredientModelFilter(this IDictionary<string, List<PropertyFilter>> propertyQueryFilters, ModelFilter modelFilter)
        {
			IngredientModelFilter ingredientModelFilter = new IngredientModelFilter(modelFilter);

			if (propertyQueryFilters.TryGetValue("searchQuery", out List<PropertyFilter> searchQueryFilters))
			{
				ingredientModelFilter.IngredientNameSnippet = StringPropertyFilterExtractor.ExtractLikeValue(searchQueryFilters);
			}

			if (propertyQueryFilters.TryGetValue("calories", out List<PropertyFilter> caloriePropertyFilters))
			{
				(float? minValue, float? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(caloriePropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinCalories = minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxCalories = minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("fat", out List<PropertyFilter> fatPropertyFilters))
			{
				(float? minValue, float? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(fatPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinFat = minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxFat = minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("salt", out List<PropertyFilter> saltPropertyFilters))
			{
				(float? minValue, float? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(saltPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinSalt = minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxSalt = minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("protein", out List<PropertyFilter> proteinPropertyFilters))
			{
				(float? minValue, float? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(proteinPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinProtein = minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxProtein = minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("carbs", out List<PropertyFilter> carbsPropertyFilters))
			{
				(float? minValue, float? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(carbsPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinCarbs = minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxCarbs = minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("fruitVeg", out List<PropertyFilter> fruitVegFilters))
			{
				bool? isFruitVeg = BooleanPropertyFilterExtractor.ExtractTruthyValue(fruitVegFilters);

				if (isFruitVeg.HasValue)
				{
					ingredientModelFilter.FruitVeg = (bool)isFruitVeg;
				}
			}

			return ingredientModelFilter;
		}
    }
}
