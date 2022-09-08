using RecipeManagerWebApi.Enums;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class StringOperationExpression : Expression
    {
        private BooleanOperationExpression _booleanOperationExpression;

        public StringOperationExpression()
        {
            _booleanOperationExpression = new BooleanOperationExpression();
        }

        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {

            string[] valueParts = propertyFilterContext.RawPropertyFilter.Split(":");

            if (valueParts.Length != 2)
            {
                return false;
            }

            string operation = valueParts[0];
            PropertyFilterOperationType propertyFilterOperationType = operation.ToFilterOperationType();

            if (!(_booleanOperationExpression.Interpret(propertyFilterContext) ||
                propertyFilterOperationType == PropertyFilterOperationType.LIKE))
            {
                return false;
            }

            propertyFilterContext.PropertyFilter.FilterOperationType = propertyFilterOperationType;
            return true;
        }
    }
}
