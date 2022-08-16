using RecipeManagerWebApi.Types.Inputs;
using RecipeManagerWebApi.Utilities;

namespace RecipeManagerWebApi.Types.Models
{
	public class IngredientModel
    {
		public IngredientModel() { }

		public IngredientModel(IngredientCreateInput ingredientCreateInput, string imageUrl)
        {
			IngredientName = ingredientCreateInput.IngredientName;
			IngredientDescription = ingredientCreateInput.IngredientDescription;
			ImageUrl = imageUrl;
            MeasureTypeId = (int)ingredientCreateInput.MeasureType.StringToMeasureType(); //TODo - should log somewhere a warning if measure type defaults to none
            Calories = ingredientCreateInput.Calories.StandardiseIngredientStatistic(ingredientCreateInput.Quantity); 
			FruitVeg = ingredientCreateInput.FruitVeg;
			Fat = ingredientCreateInput.Fat.StandardiseIngredientStatistic(ingredientCreateInput.Quantity);
			Salt = ingredientCreateInput.Salt.StandardiseIngredientStatistic(ingredientCreateInput.Quantity); 
			Protein = ingredientCreateInput.Protein.StandardiseIngredientStatistic(ingredientCreateInput.Quantity); 
			Carbs = ingredientCreateInput.Carbs.StandardiseIngredientStatistic(ingredientCreateInput.Quantity); 
		}

		public IngredientModel(IngredientUpdateInput ingredientUpdateInput, IngredientModel existingIngredientModel)
        {
			IngredientName = ingredientUpdateInput.IngredientName;
			IngredientDescription = ingredientUpdateInput.IngredientDescription;
			ImageUrl = existingIngredientModel.ImageUrl;
			MeasureTypeId = (int)ingredientUpdateInput.MeasureType.StringToMeasureType();
			Calories = ingredientUpdateInput.Calories.StandardiseIngredientStatistic(ingredientUpdateInput.Quantity);
			FruitVeg = ingredientUpdateInput.FruitVeg;
			Fat = ingredientUpdateInput.Fat.StandardiseIngredientStatistic(ingredientUpdateInput.Quantity);
			Salt = ingredientUpdateInput.Salt.StandardiseIngredientStatistic(ingredientUpdateInput.Quantity);
			Protein = ingredientUpdateInput.Protein.StandardiseIngredientStatistic(ingredientUpdateInput.Quantity);
			Carbs = ingredientUpdateInput.Carbs.StandardiseIngredientStatistic(ingredientUpdateInput.Quantity);
		}
		
		public int Id { get; set; }

		public string IngredientName { get; set; }

		public string IngredientDescription { get; set; }

		public string ImageUrl { get; set; }

		public int MeasureTypeId { get; set; }

		public float? Calories { get; set; }

		public bool FruitVeg { get; set; }

		public float? Fat { get; set; }

		public float? Salt { get; set; }

		public float? Protein { get; set; }

		public float? Carbs { get; set; }
    }
}
