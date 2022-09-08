using RecipeManagerWebApi.Types.Common;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter
{
    public class PropertyFilterContext
    {
        public PropertyFilterContext(string rawPropertyFilter)
        {
            RawPropertyFilter = rawPropertyFilter;
            PropertyFilter = new PropertyFilter();
        }

        public string RawPropertyFilter { get; private set; }

        public PropertyFilter PropertyFilter { get; set; }
    }
}
