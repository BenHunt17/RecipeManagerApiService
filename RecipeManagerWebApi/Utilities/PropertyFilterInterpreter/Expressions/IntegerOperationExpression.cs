using RecipeManagerWebApi.Enums;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class IntegerOperationExpression :  Expression
    {
        private BooleanOperationExpression _booleanOperationExpression;
        private FloatOperationExpression _floatOperationExpression;

        public IntegerOperationExpression()
        {
            _booleanOperationExpression = new BooleanOperationExpression();
            _floatOperationExpression = new FloatOperationExpression();
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
                _floatOperationExpression.Interpret(propertyFilterContext) ||
                propertyFilterOperationType == PropertyFilterOperationType.PAGE ||
                propertyFilterOperationType == PropertyFilterOperationType.GTE ||
                propertyFilterOperationType == PropertyFilterOperationType.LTE))
            {
                return false;
            }

            propertyFilterContext.PropertyFilter.FilterOperationType = propertyFilterOperationType;
            return true;
        }
    }
}
