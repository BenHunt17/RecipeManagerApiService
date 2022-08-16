using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Utilities;

namespace RecipeManagerWebApi.Tests.CustomEqualities
{
    public static class RecipeIngredientMatches
    {
        public static bool Matches(this RecipeIngredient recipeIngredient, RecipeIngredient other)
        {
            return
                recipeIngredient.IngredientName == other.IngredientName &&
                recipeIngredient.IngredientDescription == other.IngredientDescription &&
                recipeIngredient.ImageUrl == other.ImageUrl &&
                recipeIngredient.MeasureUnit == other.MeasureUnit &&
                recipeIngredient.Calories.ApproxEquals(other.Calories) &&
                recipeIngredient.FruitVeg == other.FruitVeg &&
                recipeIngredient.Salt.ApproxEquals(other.Salt) &&
                recipeIngredient.Fat.ApproxEquals(other.Fat) &&
                recipeIngredient.Protein.ApproxEquals(other.Protein) &&
                recipeIngredient.Carbs.ApproxEquals(other.Carbs) &&
                recipeIngredient.Quantity == other.Quantity;
        }
    }
}