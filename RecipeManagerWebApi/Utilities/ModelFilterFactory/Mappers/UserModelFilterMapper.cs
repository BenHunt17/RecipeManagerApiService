using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.ModelFilter;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Utilities.ModelFilterFactory.Mappers
{
    public static class UserModelFilterMapper
    {
        public static UserModelFilter MapToUserModelFilter(this IDictionary<string, List<PropertyFilter>> propertyQueryFilters, ModelFilter modelFilter)
        {
            UserModelFilter userModelFilter = new UserModelFilter(modelFilter);

            return userModelFilter;
        }
    }
}
