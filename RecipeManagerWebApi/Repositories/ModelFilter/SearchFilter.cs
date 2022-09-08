using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Utilities.PropertyFilterExtractor;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Repositories.ModelFilter
{
    public class SearchFilter
    {
        //Every filter inherits from this filter which contains the basic values needed for any query

        private const int DEFAULT_PAGE_LIMIT = 100;

        public SearchFilter()
        {
            //Constructs an "empty" search object.

            Offset = 0;
            Limit = DEFAULT_PAGE_LIMIT;
        }

        public SearchFilter(IDictionary<string, List<PropertyFilter>> propertyQueryFilters)
        {
            //Constructs search object based on the queryParmareters provided by the client

            if (propertyQueryFilters.TryGetValue("offset", out List<PropertyFilter> offsetPropertyFilters))
            {
                int? value = IntegerPropertyFilterExtractor.ExtractPaginationValue(offsetPropertyFilters);

                Offset = value ?? 0;
            }

            if (propertyQueryFilters.TryGetValue("limit", out List<PropertyFilter> limitPropertyFilters))
            {
                int? value = IntegerPropertyFilterExtractor.ExtractPaginationValue(limitPropertyFilters);
                Limit = value ?? DEFAULT_PAGE_LIMIT;
            }
        }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
