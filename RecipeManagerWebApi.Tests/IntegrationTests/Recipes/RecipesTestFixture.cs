using RecipeSchedulerApiService.Models;

namespace RecipeManagerWebApi.Tests.IntegrationTests.Ingredients
{
    public class RecipesTestFixture
    {
        public RecipeModel recipeModel { get; set; }

        public RecipesTestFixture()
        {
            recipeModel = new RecipeModel();
        }
    }
}
