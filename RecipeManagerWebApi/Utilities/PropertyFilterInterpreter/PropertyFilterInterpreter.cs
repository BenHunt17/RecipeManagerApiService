using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter
{
    public class PropertyFilterInterpreter : IPropertyFilterInterpreter
    {
        //Property filter paramters use a simple context free grammar which meant that this parsing problem could be solved with the Interpreter pattern.
        //Basically this parses the filter string values and builds and returns a big object structure which only contains filters which obey the grammar rules meaning that consumers of this can safely assume the filters are legal
        //May not be the most efficient way but it's elegant and was good to learn. It inplements the IPropertyFilterInterpreter so if needs must then it can be easily replaced anyway. Dependecy injection is magic

        private PropertyFilterExpression _propertyFilterExpression;

        public PropertyFilterInterpreter()
        {
            _propertyFilterExpression = new PropertyFilterExpression();
        }

        public IDictionary<string, List<PropertyFilter>> ParsePropertyParameters(IQueryCollection queryCollection)
        {
            //Takes all of the query paramters in string form and parses them individually using a property filter context and a recursive expression tree which builds an object
            //structure in the context as it traverses the tree and spits out a boolean at the end which indicates if the property filter parameter is valid according to the context free grammar rules.
            //This method takes the resulting object structure of each valid interpreted filter and appends them to a dictionary structure which is then returned.

            IDictionary<string, List<PropertyFilter>> propertyQueryFilters = new Dictionary<string, List<PropertyFilter>>();

            foreach (KeyValuePair<string, StringValues> queryParameters in queryCollection)
            {
                foreach(string queryParameter in queryParameters.Value)
                {
                    PropertyFilterContext propertyFilterContext = new PropertyFilterContext(queryParameter);
                    
                    if (!_propertyFilterExpression.Interpret(propertyFilterContext))
                    {
                        //Recursively traverses the expression tree and if the grammar rules aren't satisfied then just continue to next parameter
                        continue;
                    }

                    if (propertyQueryFilters.TryGetValue(queryParameters.Key, out var propertyQueryParameter))
                    {
                        //If the key value pair already exists in the structure then just append to it
                        propertyQueryParameter.Add(propertyFilterContext.PropertyFilter); //Uses the property filter context as that was modified by the expression tree to give an apropriate representation
                    }
                    else
                    {
                        //If it doesn't exist then add it
                        propertyQueryFilters.Add(queryParameters.Key, new List<PropertyFilter>() { propertyFilterContext.PropertyFilter });
                    }
                }
            }

            return propertyQueryFilters;
        }
    }
}
