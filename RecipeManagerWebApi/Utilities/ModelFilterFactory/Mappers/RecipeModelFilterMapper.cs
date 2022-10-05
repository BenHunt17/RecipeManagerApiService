using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.ModelFilter;
using RecipeManagerWebApi.Utilities.PropertyFilterExtractor;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Utilities.ModelFilterFactory.Mappers
{
    public static class RecipeModelFilterMapper
    {
		public static RecipeModelFilter MapToRecipeModelFilter(this IDictionary<string, List<PropertyFilter>> propertyQueryFilters, ModelFilter modelFilter)
		{
			RecipeModelFilter recipeModelFilter = new RecipeModelFilter(modelFilter);

			if (propertyQueryFilters.TryGetValue("recipeName", out List<PropertyFilter> recipeNameFilters))
			{
				recipeModelFilter.RecipeNameSnippet = StringPropertyFilterExtractor.ExtractLikeValue(recipeNameFilters);
			}

			if (propertyQueryFilters.TryGetValue("rating", out List<PropertyFilter> ratingPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = IntegerPropertyFilterExtractor.ExtractMinMaxValues(ratingPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					recipeModelFilter.MinRating = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					recipeModelFilter.MaxRating = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("prepTime", out List<PropertyFilter> prepTimePropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = IntegerPropertyFilterExtractor.ExtractMinMaxValues(prepTimePropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					recipeModelFilter.MinPrepTime = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					recipeModelFilter.MaxPrepTime = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("servingSize", out List<PropertyFilter> serviceSizePropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = IntegerPropertyFilterExtractor.ExtractMinMaxValues(serviceSizePropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					recipeModelFilter.MinServingSize = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					recipeModelFilter.MaxServingSize = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("breakfast", out List<PropertyFilter> breakfastPropertyFilters))
			{
				bool? isBreakfast = BooleanPropertyFilterExtractor.ExtractTruthyValue(breakfastPropertyFilters);

                if (isBreakfast.HasValue)
				{
					recipeModelFilter.Breakfast = isBreakfast;
				}
			}

			if (propertyQueryFilters.TryGetValue("lunch", out List<PropertyFilter> lunchPropertyFilters))
			{
				bool? isLunch = BooleanPropertyFilterExtractor.ExtractTruthyValue(lunchPropertyFilters);

				if (isLunch.HasValue)
				{
					recipeModelFilter.Lunch = isLunch;
				}
			}

			if (propertyQueryFilters.TryGetValue("dinner", out List<PropertyFilter> dinnerPropertyFilters))
			{
				bool? isDinner = BooleanPropertyFilterExtractor.ExtractTruthyValue(dinnerPropertyFilters);

				if (isDinner.HasValue)
				{
					recipeModelFilter.Dinner = isDinner;
				}
			}

			return recipeModelFilter;
		}
	}
}
