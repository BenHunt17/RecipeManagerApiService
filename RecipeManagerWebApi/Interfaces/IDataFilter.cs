using System.Collections.Generic;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IDataFilter<T>
    {
        //Every filter must contain at least these filters

        public int Offset { get; set; }

        public int Limit { get; set; }

        public IEnumerable<int> Ids { get; set; }

        public T Filter { get; set; }
    }
}
