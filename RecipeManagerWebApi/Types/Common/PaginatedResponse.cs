using System.Collections.Generic;

namespace RecipeManagerWebApi.Types.Common
{
    public class PaginatedResponse<T>
    {
        public PaginatedResponse(IEnumerable<T> items, int offset, int total)
        {
            Items = items;
            Offset = offset;
            Total = total;
        }

        public IEnumerable<T> Items { get; set; }

        public int Offset { get; set; }

        public int Total { get; set; }
    }
}
