using RecipeManagerWebApi.Types.Models;
using RecipeManagerWebApi.Utilities;

namespace RecipeManagerWebApi.Types.DomainObjects
{
    public class RecipeIngredient : Ingredient
    {
		//Inherits every field from the Ingredient object but also has an extra Quantity field.
		//The ingredient stats are also constructed different in that they are scaled with regard to the quantity property

		public RecipeIngredient() { }

        public RecipeIngredient(RecipeIngredientModel recipeIngredientModel, IngredientModel ingredientModel) {
			IngredientName = ingredientModel.IngredientName;
			IngredientDescription = ingredientModel.IngredientDescription;
			ImageUrl = ingredientModel.ImageUrl;
			MeasureUnit = ingredientModel.MeasureUnitId.ExtractMeasureUnit().ToMeasureUnitString();
			Calories = ingredientModel.Calories.ScaleIngredientStatistic(recipeIngredientModel.Quantity);
			FruitVeg = ingredientModel.FruitVeg;
			Fat = ingredientModel.Fat.ScaleIngredientStatistic(recipeIngredientModel.Quantity); 
			Salt = ingredientModel.Salt.ScaleIngredientStatistic(recipeIngredientModel.Quantity); 
			Protein = ingredientModel.Protein.ScaleIngredientStatistic(recipeIngredientModel.Quantity); 
			Carbs = ingredientModel.Carbs.ScaleIngredientStatistic(recipeIngredientModel.Quantity); 
			Quantity = recipeIngredientModel.Quantity;
		}

        public float Quantity { get; set; }
    }
}
