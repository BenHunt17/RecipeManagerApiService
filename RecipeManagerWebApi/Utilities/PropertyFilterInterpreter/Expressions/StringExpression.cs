using RecipeManagerWebApi.Enums;

namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class StringExpression : Expression
    {
        private IntegerExpression _integerExpression;
        private FloatExpression _floatExpression;
        private BooleanExpression _booleanExpression;

        public StringExpression()
        {
            _integerExpression = new IntegerExpression();
            _floatExpression = new FloatExpression();
            _booleanExpression = new BooleanExpression();
        }

        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            if (_integerExpression.Interpret(propertyFilterContext) ||
                _floatExpression.Interpret(propertyFilterContext) ||
                _booleanExpression.Interpret(propertyFilterContext))
            {
                //Since everything which is parsed is technically a string, I;ve made things simple and defined a filter string type as anything which isn't another type
                return false;
            }

            string[] valueParts = propertyFilterContext.RawPropertyFilter.Split(":");

            if (valueParts.Length != 2)
            {
                return false;
            }

            propertyFilterContext.PropertyFilter.Value = valueParts[1];
            propertyFilterContext.PropertyFilter.FilterDataType = PropertyFilterDataType.STRING;
            return true;
        }
    }
}
