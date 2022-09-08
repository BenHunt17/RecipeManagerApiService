using RecipeManagerWebApi.Enums;
using RecipeManagerWebApi.Types.Common;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManagerWebApi.Utilities.PropertyFilterExtractor
{
    public static class BooleanPropertyFilterExtractor
    {
        public static bool? ExtractTruthyValue(List<PropertyFilter> propertyFilters)
        {
            bool? truthy = null;

            foreach (PropertyFilter propertyFilter in propertyFilters)
            {
                //Although there should only be one filter, it naively iterates over all of them and will just overwrite the truthy value

                if (bool.TryParse(propertyFilter.Value, out bool value))
                {
                    //If the operation type is "NEQ" then the boolean value to filter by will logically be negated
                    truthy = propertyFilter.FilterOperationType == PropertyFilterOperationType.EQ
                        ? value : !value;
                }
            }

            return truthy;
        }
    }
}
