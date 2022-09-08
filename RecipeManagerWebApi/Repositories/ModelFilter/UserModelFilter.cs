using RecipeManagerWebApi.Repositories.ModelFilter;
using RecipeManagerWebApi.Types.Common;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Repositories.ModelFilter
{
    public class UserModelFilter : SearchFilter
    {
        public UserModelFilter(IDictionary<string, List<PropertyFilter>> propertyQueryFilters) : base(propertyQueryFilters)
        {

        }
    }
}
