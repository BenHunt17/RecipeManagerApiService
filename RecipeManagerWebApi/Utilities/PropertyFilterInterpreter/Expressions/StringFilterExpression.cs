namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class StringFilterExpression : Expression
    {
        private StringOperationExpression _stringOperationExpression;
        private StringExpression _stringExpression;

        public StringFilterExpression()
        {
            _stringOperationExpression = new StringOperationExpression();
            _stringExpression = new StringExpression(); 
        }

        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            return _stringOperationExpression.Interpret(propertyFilterContext) &&
                   _stringExpression.Interpret(propertyFilterContext);
        }
    }
}
