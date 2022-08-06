using RecipeSchedulerApiService.Types.Inputs;
using RecipeSchedulerApiService.Utilities;
using System;

namespace RecipeSchedulerApiService.Models
{
	public class IngredientModel
    {
		public IngredientModel() { }

		public IngredientModel(IngredientCreateInput ingredientCreateInput, string imageUrl)
        {
			//Constructor for constructing an ingredient model using the ingredient input type.

			IngredientName = ingredientCreateInput.IngredientName;
			IngredientDescription = ingredientCreateInput.IngredientDescription;
			ImageUrl = imageUrl;
			MeasureType = ingredientCreateInput.MeasureType;
			Calories = ingredientCreateInput.Calories;
			FruitVeg = ingredientCreateInput.FruitVeg;
			Fat = ingredientCreateInput.Fat;
			Salt = ingredientCreateInput.Salt;
			Protein = ingredientCreateInput.Protein;
			Carbs = ingredientCreateInput.Carbs;
		}

		public IngredientModel(IngredientUpdateInput ingredientUpdateInput)
        {
			IngredientName = ingredientUpdateInput.IngredientName;
			IngredientDescription = ingredientUpdateInput.IngredientDescription;
			MeasureType = ingredientUpdateInput.MeasureType;
			Calories = ingredientUpdateInput.Calories;
			FruitVeg = ingredientUpdateInput.FruitVeg;
			Fat = ingredientUpdateInput.Fat;
			Salt = ingredientUpdateInput.Salt;
			Protein = ingredientUpdateInput.Protein;
			Carbs = ingredientUpdateInput.Carbs;
		}
		
		public int Id { get; set; }

		public string IngredientName { get; set; }

		public string IngredientDescription { get; set; }

		public string ImageUrl { get; set; }

		public int MeasureTypeId { get; set; }

		public string MeasureType { get; set; }

		public float? Calories { get; set; }

		public bool FruitVeg { get; set; }

		public float? Fat { get; set; }

		public float? Salt { get; set; }

		public float? Protein { get; set; }

		public float? Carbs { get; set; }

        public override bool Equals(Object obj)
        {
			IngredientModel ingredientModel = obj as IngredientModel;

			return
				IngredientName == ingredientModel.IngredientName &&
				IngredientDescription == ingredientModel.IngredientDescription &&
				ImageUrl == ingredientModel.ImageUrl &&
				MeasureType == ingredientModel.MeasureType &&
				Calories.ApproxEquals(ingredientModel.Calories) &&
				FruitVeg == ingredientModel.FruitVeg &&
				Fat.ApproxEquals(ingredientModel.Fat) &&
				Salt.ApproxEquals(ingredientModel.Salt) &&
				Protein.ApproxEquals(ingredientModel.Protein) &&
				Carbs.ApproxEquals(ingredientModel.Carbs);
		}
    }
}
