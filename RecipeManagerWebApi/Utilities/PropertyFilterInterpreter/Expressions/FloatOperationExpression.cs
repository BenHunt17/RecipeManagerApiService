using RecipeManagerWebApi.Enums;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class FloatOperationExpression : Expression
    {
        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            string[] valueParts = propertyFilterContext.RawPropertyFilter.Split(":");

            if (valueParts.Length != 2)
            {
                return false;
            }

            string operation = valueParts[0];
            PropertyFilterOperationType propertyFilterOperationType = operation.ToFilterOperationType();

            if (!(propertyFilterOperationType == PropertyFilterOperationType.GT ||
                propertyFilterOperationType == PropertyFilterOperationType.LT))
            {
                return false;
            }

            propertyFilterContext.PropertyFilter.FilterOperationType = propertyFilterOperationType;
            return true;
        }
    }
}
