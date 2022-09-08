using RecipeManagerWebApi.Enums;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class BooleanExpression : Expression
    {
        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            string[] valueParts = propertyFilterContext.RawPropertyFilter.Split(":");

            if (valueParts.Length != 2)
            {
                return false;
            }

            bool value;
            if (bool.TryParse(valueParts[1], out value))
            {
                propertyFilterContext.PropertyFilter.Value = value.ToString();
                propertyFilterContext.PropertyFilter.FilterDataType = PropertyFilterDataType.BOOLEAN;
                return true;
            }

            return false;
        }
    }
}
