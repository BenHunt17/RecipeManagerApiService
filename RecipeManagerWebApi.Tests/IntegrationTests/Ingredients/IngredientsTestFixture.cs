using RecipeManagerWebApi.Types.DomainObjects;

namespace RecipeManagerWebApi.Tests.IntegrationTests.Ingredients
{
    public class IngredientsTestFixture
    {
        //Very simple fixture with the only purpose being to persist an ingredient model of interest between test cases

        public Ingredient ingredient { get; set; }

        public IngredientsTestFixture()
        {
            ingredient = new Ingredient();
        }
    }
}
