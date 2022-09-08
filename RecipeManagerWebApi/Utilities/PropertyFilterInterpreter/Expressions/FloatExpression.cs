using RecipeManagerWebApi.Enums;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class FloatExpression : Expression
    {
        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            string[] valueParts = propertyFilterContext.RawPropertyFilter.Split(":");

            if (valueParts.Length != 2)
            {
                return false;
            }

            float value;
            if (float.TryParse(valueParts[1], out value))
            {
                propertyFilterContext.PropertyFilter.Value = value.ToString();
                propertyFilterContext.PropertyFilter.FilterDataType = PropertyFilterDataType.FLOAT;
                return true;
            }

            return false;
        }
    }
}
