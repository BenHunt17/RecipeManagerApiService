using RecipeManagerWebApi.Enums;
using RecipeManagerWebApi.Types.Common;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManagerWebApi.Utilities.PropertyFilterExtractor
{
    public static class StringPropertyFilterExtractor
    {
        public static string ExtractEqualValue(IEnumerable<PropertyFilter> propertyFilters)
        {
            PropertyFilter propertyFilter = propertyFilters.FirstOrDefault(propertyFilter =>
                propertyFilter.FilterOperationType == PropertyFilterOperationType.EQ);

            return propertyFilter.Value;
        }
        public static string ExtractNotEqualValue(IEnumerable<PropertyFilter> propertyFilters)
        {
            PropertyFilter propertyFilter = propertyFilters.FirstOrDefault(propertyFilter =>
                propertyFilter.FilterOperationType == PropertyFilterOperationType.NEQ);

            return propertyFilter.Value;
        }

        public static string ExtractLikeValue(IEnumerable<PropertyFilter> propertyFilters)
        {
            PropertyFilter propertyFilter = propertyFilters.FirstOrDefault(propertyFilter =>
                propertyFilter.FilterOperationType == PropertyFilterOperationType.LIKE);

            return propertyFilter.Value;
        }
    }
}
