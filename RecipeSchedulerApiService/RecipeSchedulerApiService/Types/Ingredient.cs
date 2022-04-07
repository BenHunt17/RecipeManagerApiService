using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Types
{
    public class Ingredient
    {
		public Ingredient(IngredientModel ingredientModel)
        {
			Id = ingredientModel.Id;
			IngredientName = ingredientModel.IngredientName;
			IngredientDescription = ingredientModel.IngredientDescription;
			ImageUrl = ingredientModel.ImageUrl;
			Density = ingredientModel.Density;
			Calories = ingredientModel.Calories;
			IngredientName = ingredientModel.IngredientName;
			FruitVeg = ingredientModel.FruitVeg;
			Fat = ingredientModel.Fat;
			Salt = ingredientModel.Salt;
			Protein = ingredientModel.Protein;
			Carbs = ingredientModel.Carbs;
		}

        public int Id { get; set; }

		public string IngredientName { get; set; }

		public string IngredientDescription { get; set; }

		public string ImageUrl { get; set; }

		public float? Density { get; set; }

		public float? Calories { get; set; }

		public bool FruitVeg { get; set; }

		public float? Fat { get; set; }

		public float? Salt { get; set; }

		public float? Protein { get; set; }

		public float? Carbs { get; set; }
	}
}
