using RecipeSchedulerApiService.Models;

namespace RecipeManagerWebApi.Tests.Fixtures
{
    public class IngredientsTestFixture
    {
        //Very simple fixture with the only purpose being to persist an ingredient model of interest between test cases

        public IngredientModel ingredientModel { get; set; }

        public IngredientsTestFixture()
        {
            ingredientModel = new IngredientModel();
        }
    }
}
