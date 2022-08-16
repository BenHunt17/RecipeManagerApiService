using RecipeManagerWebApi.Types.DomainObjects;

namespace RecipeManagerWebApi.Tests.IntegrationTests.Ingredients
{
    public class RecipesTestFixture
    {
        public Recipe recipe { get; set; }

        public RecipesTestFixture()
        {
            recipe = new Recipe();
        }
    }
}
