namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class BooleanFilterExpression : Expression
    {
        private BooleanOperationExpression _booleanOperationExpression;
        private BooleanExpression _booleanExpression;

        public BooleanFilterExpression()
        {
            _booleanOperationExpression = new BooleanOperationExpression();
            _booleanExpression = new BooleanExpression();   
        }

        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            return _booleanOperationExpression.Interpret(propertyFilterContext) && 
                   _booleanExpression.Interpret(propertyFilterContext);
        }
    }
}
