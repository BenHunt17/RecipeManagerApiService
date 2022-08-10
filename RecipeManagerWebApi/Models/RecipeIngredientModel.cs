using RecipeSchedulerApiService.Types.Inputs;
using RecipeSchedulerApiService.Utilities;

namespace RecipeSchedulerApiService.Models
{
    public class RecipeIngredientModel : IngredientModel
    {
        public RecipeIngredientModel () { }

        public RecipeIngredientModel(RecipeIngredientInput recipeIngredientInput)
        {
            IngredientId = recipeIngredientInput.IngredientId;
            Quantity = recipeIngredientInput.Quantity;
        }

        public int IngredientId { get; set; }

		public float Quantity { get; set; }

        public bool CompareInput(RecipeIngredientModel recipeIngredientModel)
        {
            //This method takes in a recipe ingredient model which is assumed to have a lot of null fields due to it being mapped using the ingredient input constructor above.
            //This method just checks the important fields and returns whether it's the same recipe ingredient

            return IngredientId == recipeIngredientModel.IngredientId && Quantity == recipeIngredientModel.Quantity;
        }

        public override bool Equals(object obj)
        {
            RecipeIngredientModel recipeIngredientModel = obj as RecipeIngredientModel;

            if (recipeIngredientModel == null)
            {
                return false;
            }

            return
                IngredientId == recipeIngredientModel.IngredientId &&
                Quantity == recipeIngredientModel.Quantity &&
                IngredientName == recipeIngredientModel.IngredientName &&
                IngredientDescription == recipeIngredientModel.IngredientDescription &&
                ImageUrl == recipeIngredientModel.ImageUrl &&
                MeasureType == recipeIngredientModel.MeasureType &&
                Calories.ApproxEquals(recipeIngredientModel.Calories) &&
                FruitVeg == recipeIngredientModel.FruitVeg &&
                Fat.ApproxEquals(recipeIngredientModel.Fat) &&
                Salt.ApproxEquals(recipeIngredientModel.Salt) &&
                Protein.ApproxEquals(recipeIngredientModel.Protein) &&
                Carbs.ApproxEquals(recipeIngredientModel.Carbs);
        }
    }
}

