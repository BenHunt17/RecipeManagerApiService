using RecipeManagerWebApi.Enums;

namespace RecipeManagerWebApi.Types.Common
{
    public class PropertyFilter
    {
        public string Value { get; set; }

        public PropertyFilterDataType FilterDataType { get; set; }

        public PropertyFilterOperationType FilterOperationType { get; set; }
    }
}
