using RecipeSchedulerApiService.Types.Inputs;
using RecipeSchedulerApiService.Utilities;

namespace RecipeSchedulerApiService.Models
{
	public class IngredientModel
    {
		public IngredientModel() { }

		public IngredientModel(IngredientCreateInput ingredientCreateInput, string imageUrl)
        {
			//Constructor for constructing an ingredient model using the ingredient input type. The nutrional stats are all standardised using the quanitity provided in the input so that they are all stored as the common unit used in the database

			IngredientName = ingredientCreateInput.IngredientName;
			IngredientDescription = ingredientCreateInput.IngredientDescription;
			ImageUrl = imageUrl;
			Density = ingredientCreateInput.Density;
			QuantityTypeValue = ingredientCreateInput.QuantityType;
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
			Density = ingredientUpdateInput.Density;
			QuantityTypeValue = ingredientUpdateInput.QuantityType;
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

		public float? Density { get; set; } //TODO - This field is essential in the standardisation of some ingredients. need to investigate how to handle it better

		public string QuantityTypeValue { get; set; }

		public float? Calories { get; set; }

		public bool FruitVeg { get; set; }

		public float? Fat { get; set; }

		public float? Salt { get; set; }

		public float? Protein { get; set; }

		public float? Carbs { get; set; }
	}
}
