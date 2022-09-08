using RecipeManagerWebApi.Enums;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class IntegerExpression : Expression
    {
        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            string[] valueParts = propertyFilterContext.RawPropertyFilter.Split(":");

            if (valueParts.Length != 2)
            {
                return false;
            }

            int value;
            if (int.TryParse(valueParts[1], out value))
            {
                propertyFilterContext.PropertyFilter.Value = value.ToString();
                propertyFilterContext.PropertyFilter.FilterDataType = PropertyFilterDataType.INTEGER;
                return true;
            }

            return false;
        }
    }
}
