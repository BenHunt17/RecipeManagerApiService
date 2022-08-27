namespace RecipeManagerWebApi.Repositories.ModelSearch
{
    public class IngredientModelFilter
    {
		public string IngredientNameSnippet { get; set; }

		public float MinCalories { get; set; }

		public float MaxCalories { get; set; }

		public float MinFat { get; set; }

		public float MaxFat { get; set; }

		public float MinSalt { get; set; }

		public float MaxSalt { get; set; }

		public float MinProtein { get; set; }

		public float MaxProtein { get; set; }

		public float MinCarbs { get; set; }

		public float MaxCarbs { get; set; }

		public bool FruitVeg { get; set; }
	}
}
