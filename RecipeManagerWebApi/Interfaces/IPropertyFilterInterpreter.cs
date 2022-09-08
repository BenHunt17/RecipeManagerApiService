using Microsoft.AspNetCore.Http;
using RecipeManagerWebApi.Types.Common;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IPropertyFilterInterpreter
    {
        IDictionary<string, List<PropertyFilter>> ParsePropertyParameters(IQueryCollection queryCollection);
    }
}
