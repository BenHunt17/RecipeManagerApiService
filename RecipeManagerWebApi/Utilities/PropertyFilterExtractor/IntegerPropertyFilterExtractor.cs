using RecipeManagerWebApi.Enums;
using RecipeManagerWebApi.Types.Common;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManagerWebApi.Utilities.PropertyFilterExtractor
{
    public static class IntegerPropertyFilterExtractor
    {
        //Basically gives context to the property filters. Allows consumers to extract paramters which match the desired business context
        //Some methods actually build new property values for certain operation types which are logically equal to othe rpotential values in the filters. 
        //This allows for the client to have more freedom while still having a relatively small amount of model filter values 

        public static int? ExtractPaginationValue(IEnumerable<PropertyFilter> propertyFilters)
        {
            PropertyFilter propertyFilter = propertyFilters.FirstOrDefault(propertyFilter =>
                propertyFilter.FilterOperationType == PropertyFilterOperationType.PAGE);

            if (int.TryParse(propertyFilter.Value, out int paginationValue)) {
                return paginationValue;
            }

            return null;
        }

        public static (int? minValue, int? maxValue) ExtractMinMaxValues(IEnumerable<PropertyFilter> propertyFilters)
        {
            //The integer filter type is legally allowed a variety of operation types including "equals", "less than" or "less than or equal to" etc.
            //However, it turns out that filters of these operation types can be represented using just "less than" and "greater than" values
            //Therefore in order to save myself making bloated filter model classes and even more hideous stored procedures, I can just extract a min and max value
            //Which logicially have the same result as many different operation types the client may have used.
            //When it comes to conflicting filters then this method will naively just carry on which would obviously lead to some incorrect output.
            //However if the client gives conflcting values then it can't expect correct results anyway since logically there is no correct result for conflicting filters

            int? minValue = null;
            int? maxValue = null;

            foreach (PropertyFilter propertyFilter in propertyFilters) {
                if(int.TryParse(propertyFilter.Value, out int value))
                {
                    switch (propertyFilter.FilterOperationType)
                    {
                        case PropertyFilterOperationType.GT:
                            minValue = value;
                            break;
                        case PropertyFilterOperationType.LT:
                            maxValue = value;
                            break;
                        case PropertyFilterOperationType.EQ:
                            //If the operation is equal then as the value is an integer, a number equal to the value is the same as a number
                            //which sits between the value's neighbours. i.e. n == 7 is the same thing as (n > 6 && n < 8)
                            minValue = value - 1;
                            maxValue = value + 1;
                            break;
                        case PropertyFilterOperationType.GTE:
                            minValue = value - 1;
                            break;
                        case PropertyFilterOperationType.LTE:
                            maxValue = value + 1;
                            break;
                    }
                } 
            }

            return (minValue, maxValue);
        }

        public static int? ExtractNotEqualValue(IEnumerable<PropertyFilter> propertyFilters)
        {
            PropertyFilter propertyFilter = propertyFilters.FirstOrDefault(propertyFilter =>
                propertyFilter.FilterOperationType == PropertyFilterOperationType.NEQ);

            if (int.TryParse(propertyFilter.Value, out int notEqualValue))
            {
                return notEqualValue;
            }

            return null;
        }
    }
}
