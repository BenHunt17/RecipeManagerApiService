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

            if (propertyFilter == null)
            {
                return null;
            }

            return propertyFilter.Value;
        }
        public static string ExtractNotEqualValue(IEnumerable<PropertyFilter> propertyFilters)
        {
            PropertyFilter propertyFilter = propertyFilters.FirstOrDefault(propertyFilter =>
                propertyFilter.FilterOperationType == PropertyFilterOperationType.NEQ);

            if (propertyFilter == null)
            {
                return null;
            }

            return propertyFilter.Value;
        }

        public static string ExtractLikeValue(IEnumerable<PropertyFilter> propertyFilters)
        {
            PropertyFilter propertyFilter = propertyFilters.FirstOrDefault(propertyFilter =>
                propertyFilter.FilterOperationType == PropertyFilterOperationType.LIKE);

            if (propertyFilter == null)
            {
                return null;
            }

            return propertyFilter.Value;
        }
    }
}
