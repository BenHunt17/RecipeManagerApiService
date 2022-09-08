using RecipeManagerWebApi.Enums;
using RecipeManagerWebApi.Types.Common;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Utilities.PropertyFilterExtractor
{
    public static class FloatPropertyFilterExtractor
    {
        public static (int? minValue, int? maxValue) ExtractMinMaxValues(List<PropertyFilter> propertyFilters)
        {
            // From a business prospective in this application, it doesn't really make sense for two floats to be equal because no ingredient/recipe are going to have exactly the same aamount of calories or whatever.
            // Therefore for this application only 'LT' and 'GT' are valid operations on floats

            int? minValue = null;
            int? maxValue = null;

            foreach (PropertyFilter propertyFilter in propertyFilters)
            {
                if (int.TryParse(propertyFilter.Value, out int value))
                {
                    switch (propertyFilter.FilterOperationType)
                    {
                        case PropertyFilterOperationType.GT:
                            minValue = value;
                            break;
                        case PropertyFilterOperationType.LT:
                            maxValue = value;
                            break;
                    }
                }
            }

            return (minValue, maxValue);
        }
    }
}
