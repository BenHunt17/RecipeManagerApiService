using RecipeManagerWebApi.Types.Models;
using RecipeManagerWebApi.Utilities;

namespace RecipeManagerWebApi.Types.DomainObjects
{
    public class Ingredient
	{
		public Ingredient() { }

		public Ingredient(IngredientModel ingredientModel)
        {
			IngredientName = ingredientModel.IngredientName;
			IngredientDescription = ingredientModel.IngredientDescription;
			ImageUrl = ingredientModel.ImageUrl;
			MeasureUnit = ingredientModel.MeasureUnitId.ExtractMeasureUnit().ToMeasureUnitString(); 
			Calories = ingredientModel.Calories;
			FruitVeg = ingredientModel.FruitVeg;
			Fat = ingredientModel.Fat;
			Salt = ingredientModel.Salt;
			Protein = ingredientModel.Protein;
			Carbs = ingredientModel.Carbs;
		}

		public string IngredientName { get; set; }

		public string IngredientDescription { get; set; }

		public string ImageUrl { get; set; }

		public string MeasureUnit { get; set; }

		public float? Calories { get; set; }

		public bool FruitVeg { get; set; }

		public float? Fat { get; set; }

		public float? Salt { get; set; }

		public float? Protein { get; set; }

		public float? Carbs { get; set; }
	}
}
