namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class FloatFilterExpression : Expression
    {
        private FloatOperationExpression _floatOperationExpression;
        private FloatExpression _floatExpression;

        public FloatFilterExpression()
        {
            _floatOperationExpression = new FloatOperationExpression();
            _floatExpression = new FloatExpression();
        }

        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            return _floatOperationExpression.Interpret(propertyFilterContext) &&
                   _floatExpression.Interpret(propertyFilterContext);
        }
    }
}
