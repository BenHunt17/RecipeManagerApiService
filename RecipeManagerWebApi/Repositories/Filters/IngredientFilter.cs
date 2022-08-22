namespace RecipeManagerWebApi.Repositories.Filters
{
    public class IngredientFilter
    {
		string IngredientNameSnippet { get; set; }

		float MinCalories { get; set; }

		float MaxCalories { get; set; }

		float MinFat { get; set; }

		float MaxFat { get; set; }

		float MinSalt { get; set; }

		float MaxSalt { get; set; }

		float MinProtein { get; set; }

		float MaxProtein { get; set; }

		float MinCarbs { get; set; }

		float MaxCarbs { get; set; }

		bool FruitVeg { get; set; }
	}
}
