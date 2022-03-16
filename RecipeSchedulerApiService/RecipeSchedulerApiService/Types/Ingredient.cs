namespace RecipeSchedulerApiService.Types
{
    public class Ingredient
    {
		public int Id { get; set; }

		public string IngredientName { get; set; }

		public string IngredientDescription { get; set; }

		public string ImageUrl { get; set; }

		public float Calories { get; set; }

		public bool FruitVeg { get; set; }

		public float Fat { get; set; }

		public float Salt { get; set; }

		public float Protein { get; set; }

		public float Carbs { get; set; }
	}
}
