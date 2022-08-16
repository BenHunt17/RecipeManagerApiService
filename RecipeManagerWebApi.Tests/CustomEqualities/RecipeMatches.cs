using RecipeManagerWebApi.Types.DomainObjects;
using System.Linq;

namespace RecipeManagerWebApi.Tests.CustomEqualities
{
    public static class RecipeMatches
    {
        public static bool Matches(this Recipe recipe, Recipe other)
        {
            return
                recipe.RecipeName == other.RecipeName &&
                recipe.RecipeDescription == other.RecipeDescription &&
                recipe.ImageUrl == other.ImageUrl &&
                recipe.Ingredients.All(recipeIngredient => other.Ingredients.Any(otherRecipeIngredient => recipeIngredient.Matches(otherRecipeIngredient))) &&
                recipe.Instructions.All(instruction => other.Instructions.Any(otherInstruction => instruction.Matches(otherInstruction))) &&
                recipe.Rating == other.Rating &&
                recipe.PrepTime == other.PrepTime &&
                recipe.ServingSize == other.ServingSize &&
                recipe.Breakfast == other.Breakfast &&
                recipe.Lunch == other.Lunch &&
                recipe.Dinner == other.Dinner;
        }
    }
}