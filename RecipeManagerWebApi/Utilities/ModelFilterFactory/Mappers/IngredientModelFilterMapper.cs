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

			if (propertyQueryFilters.TryGetValue("ingredientName", out List<PropertyFilter> ingredientNameFilters))
			{
				ingredientModelFilter.IngredientNameSnippet = StringPropertyFilterExtractor.ExtractLikeValue(ingredientNameFilters);
			}

			if (propertyQueryFilters.TryGetValue("calories", out List<PropertyFilter> caloriePropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(caloriePropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinCalories = (float)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxCalories = (float)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("fat", out List<PropertyFilter> fatPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(fatPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinFat = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxFat = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("salt", out List<PropertyFilter> saltPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(saltPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinSalt = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxSalt = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("protein", out List<PropertyFilter> proteinPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(proteinPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinProtein = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxProtein = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("carbs", out List<PropertyFilter> carbsPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(carbsPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					ingredientModelFilter.MinCarbs = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					ingredientModelFilter.MaxCarbs = (int)minMaxValues.maxValue;
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
