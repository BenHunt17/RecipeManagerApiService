namespace RecipeManagerWebApi.Utilities.PropertyFilterInterpreter.Expressions
{
    public abstract class Expression
    {
        public abstract bool Interpret(PropertyFilterContext propertyFilterContext);
    }
}
