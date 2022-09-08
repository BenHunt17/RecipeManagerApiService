using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Utilities.PropertyFilterExtractor;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Repositories.ModelFilter
{
    public class IngredientModelFilter : SearchFilter
	{
		public IngredientModelFilter(IDictionary<string, List<PropertyFilter>> propertyQueryFilters) : base(propertyQueryFilters)
        {
			if(propertyQueryFilters.TryGetValue("ingredientName", out List<PropertyFilter> ingredientNameFilters)) 
			{
				IngredientNameSnippet = StringPropertyFilterExtractor.ExtractLikeValue(ingredientNameFilters);
            }

			if (propertyQueryFilters.TryGetValue("calories", out List<PropertyFilter> caloriePropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(caloriePropertyFilters);

				if (minMaxValues.minValue.HasValue)
                {
					MinCalories = (float)minMaxValues.minValue;
                }
                if (minMaxValues.maxValue.HasValue)
                {
					MaxCalories = (float)minMaxValues.maxValue;
                }
			}

			if (propertyQueryFilters.TryGetValue("fat", out List<PropertyFilter> fatPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(fatPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					MinFat = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					MaxFat = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("salt", out List<PropertyFilter> saltPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(saltPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					MinSalt = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					MaxSalt = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("protein", out List<PropertyFilter> proteinPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(proteinPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					MinProtein = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					MaxProtein = (int)minMaxValues.maxValue;
				}
			}

			if (propertyQueryFilters.TryGetValue("carbs", out List<PropertyFilter> carbsPropertyFilters))
			{
				(int? minValue, int? maxValue) minMaxValues = FloatPropertyFilterExtractor.ExtractMinMaxValues(carbsPropertyFilters);

				if (minMaxValues.minValue.HasValue)
				{
					MinCarbs = (int)minMaxValues.minValue;
				}
				if (minMaxValues.maxValue.HasValue)
				{
					MaxCarbs = (int)minMaxValues.maxValue;
				}
			}

			if(propertyQueryFilters.TryGetValue("fruitVeg", out List<PropertyFilter> fruitVegFilters))
            {
				bool? isFruitVeg = BooleanPropertyFilterExtractor.ExtractTruthyValue(fruitVegFilters);

                if (isFruitVeg.HasValue)
                {
					FruitVeg = (bool)isFruitVeg;
                }
            }
		}

		public string IngredientNameSnippet { get; set; }

		public float? MinCalories { get; set; }

		public float? MaxCalories { get; set; }

		public float? MinFat { get; set; }

		public float? MaxFat { get; set; }

		public float? MinSalt { get; set; }

		public float? MaxSalt { get; set; }

		public float? MinProtein { get; set; }

		public float? MaxProtein { get; set; }

		public float? MinCarbs { get; set; }

		public float? MaxCarbs { get; set; }

		public bool? FruitVeg { get; set; }
	}
}
