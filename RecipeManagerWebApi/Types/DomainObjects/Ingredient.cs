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
			MeasureUnit = ingredientModel.MeasureTypeId.ExtractMeasureType().ToMeasureTypeString(); 
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

		//TODO - Shouldn't need to override anymore since all fields are relevant
		//public override bool Equals(object obj)
		//{
		//	Ingredient ingredient = obj as Ingredient;

		//	if (ingredient == null)
		//	{
		//		return false;
		//	}

		//	return
		//		IngredientName == ingredient.IngredientName &&
		//		IngredientDescription == ingredient.IngredientDescription &&
		//		ImageUrl == ingredient.ImageUrl &&
		//		Calories.ApproxEquals(ingredient.Calories) &&
		//		FruitVeg == ingredient.FruitVeg &&
		//		Fat.ApproxEquals(ingredient.Fat) &&
		//		Salt.ApproxEquals(ingredient.Salt) &&
		//		Protein.ApproxEquals(ingredient.Protein) &&
		//		Carbs.ApproxEquals(ingredient.Carbs);
		//}
	}
}
