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
			Calories = ingredientCreateInput.Calories != null ? IngredientUtilities.StandardiseAmount((float)ingredientCreateInput.Calories, ingredientCreateInput.Quantity, EnumUtilities.StringToQuantityType(ingredientCreateInput.QuantityType)) : null; //Calls the standardise amount helper method to convert the amount to the database common unit based on the quantity given by the user.
			FruitVeg = ingredientCreateInput.FruitVeg;
			Fat = ingredientCreateInput.Fat != null ? IngredientUtilities.StandardiseAmount((float)ingredientCreateInput.Fat, ingredientCreateInput.Quantity, EnumUtilities.StringToQuantityType(ingredientCreateInput.QuantityType)) : null;
			Salt = ingredientCreateInput.Salt != null ? IngredientUtilities.StandardiseAmount((float)ingredientCreateInput.Salt, ingredientCreateInput.Quantity, EnumUtilities.StringToQuantityType(ingredientCreateInput.QuantityType)) : null;
			Protein = ingredientCreateInput.Protein != null ? IngredientUtilities.StandardiseAmount((float)ingredientCreateInput.Protein, ingredientCreateInput.Quantity, EnumUtilities.StringToQuantityType(ingredientCreateInput.QuantityType)) : null;
			Carbs = ingredientCreateInput.Carbs != null ? IngredientUtilities.StandardiseAmount((float)ingredientCreateInput.Carbs, ingredientCreateInput.Quantity, EnumUtilities.StringToQuantityType(ingredientCreateInput.QuantityType)) : null;
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
