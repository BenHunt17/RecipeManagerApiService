using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.ModelFilter;
using RecipeManagerWebApi.Utilities.PropertyFilterExtractor;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Utilities.ModelFilterFactory.Mappers
{
    public static class ModelFilterMapper
    {
        public static ModelFilter MapToModelFilter(this IDictionary<string, List<PropertyFilter>> propertyQueryFilters)
        {
            ModelFilter modelFilter = new ModelFilter();

            if (propertyQueryFilters.TryGetValue("offset", out List<PropertyFilter> offsetPropertyFilters))
            {
                int? value = IntegerPropertyFilterExtractor.ExtractPaginationValue(offsetPropertyFilters);

                modelFilter.Offset = value ?? 0;
            }

            if (propertyQueryFilters.TryGetValue("limit", out List<PropertyFilter> limitPropertyFilters))
            {
                int? value = IntegerPropertyFilterExtractor.ExtractPaginationValue(limitPropertyFilters);

                if(value != null)
                {
                    modelFilter.Limit = (int)value;
                }
            }

            return modelFilter;
        }
    }
}
