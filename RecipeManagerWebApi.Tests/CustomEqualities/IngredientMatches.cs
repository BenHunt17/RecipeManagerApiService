using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Utilities;

namespace RecipeManagerWebApi.Tests.CustomEqualities
{
    public static class IngredientMatches
    {
        public static bool Matches(this Ingredient ingredient, Ingredient other)
        {
            return
                ingredient.IngredientName == other.IngredientName &&
                ingredient.IngredientDescription == other.IngredientDescription &&
                ingredient.ImageUrl == other.ImageUrl &&
                ingredient.MeasureUnit == other.MeasureUnit &&
                ingredient.Calories.ApproxEquals(other.Calories) &&
                ingredient.FruitVeg == other.FruitVeg &&
                ingredient.Salt.ApproxEquals(other.Salt) &&
                ingredient.Fat.ApproxEquals(other.Fat) &&
                ingredient.Protein.ApproxEquals(other.Protein) &&
                ingredient.Carbs.ApproxEquals(other.Carbs);
        }
    }
}
