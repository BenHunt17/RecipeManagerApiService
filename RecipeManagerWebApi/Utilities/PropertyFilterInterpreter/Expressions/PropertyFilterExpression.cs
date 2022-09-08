namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public class PropertyFilterExpression : Expression
    {
        private IntegerFilterExpression _integerFilterExpression;
        private FloatFilterExpression _floatFilterExpression;
        private StringFilterExpression _stringFilterExpression;
        private BooleanFilterExpression _booleanFilterExpression;

        public PropertyFilterExpression()
        {
            _integerFilterExpression = new IntegerFilterExpression();
            _floatFilterExpression = new FloatFilterExpression();
            _stringFilterExpression = new StringFilterExpression();
            _booleanFilterExpression = new BooleanFilterExpression();
        }

        public override bool Interpret(PropertyFilterContext propertyFilterContext)
        {
            return _integerFilterExpression.Interpret(propertyFilterContext) ||
                   _floatFilterExpression.Interpret(propertyFilterContext) ||
                   _stringFilterExpression.Interpret(propertyFilterContext) ||
                   _booleanFilterExpression.Interpret(propertyFilterContext);
        }
    }
}
