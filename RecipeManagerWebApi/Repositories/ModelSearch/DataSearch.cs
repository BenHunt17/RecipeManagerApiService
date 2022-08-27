using System.Collections.Generic;

namespace RecipeManagerWebApi.Repositories.ModelSearch
{
    public class DataSearch<T>
    {
        //Every filter inherits from this filter which contains the basic values needed for any query

        private const int DEFAULT_PAGE_LIMIT = 100;

        public DataSearch()
        {
            //Constructs an "empty" search object.

            Offset = 0;
            Limit = DEFAULT_PAGE_LIMIT;
            Ids = new List<int>();
            NaturalKeys = new List<string>();
            ModelFilter = default(T);
        }

        public DataSearch(int offset, int limit, IEnumerable<int> ids, IEnumerable<string> naturalKeys, T modelFilter)
        {
            Offset = offset; 
            Limit = limit; 
            Ids = ids;
            NaturalKeys = naturalKeys;

            if (modelFilter != null)
            {
                ModelFilter = modelFilter;
            }
            else
            {
                ModelFilter = default(T);
            }
        }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public IEnumerable<int> Ids { get; set; }

        public IEnumerable<string> NaturalKeys { get; set; }

        public T ModelFilter { get; set; }
    }
}
