using RecipeManagerWebApi.Types.Inputs;

namespace RecipeManagerWebApi.Types.Models
{
    public class RecipeIngredientModel
    {
        public RecipeIngredientModel() { }

        public RecipeIngredientModel(RecipeIngredientInput recipeIngredientInput, int ingredientId)
        {
            IngredientId = ingredientId;
            Quantity = recipeIngredientInput.Quantity;
        }

        public int Id { get; set; }

        public int IngredientId { get; set; }

        public float Quantity { get; set; }

        public int RecipeId { get; set; }
    }
}

