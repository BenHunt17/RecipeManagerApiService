namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class IntegerFilterExpression : Expression
    {
        private IntegerOperationExpression _integerOperationExpression;
        private IntegerExpression _integerExpression;

        public IntegerFilterExpression()
        {
            _integerOperationExpression = new IntegerOperationExpression();
            _integerExpression = new IntegerExpression();
        }

        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            return _integerOperationExpression.Interpret(propertyFilterContext) &&
                   _integerExpression.Interpret(propertyFilterContext);
        }
    }
}
