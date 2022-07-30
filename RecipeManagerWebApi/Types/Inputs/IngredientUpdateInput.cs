using Microsoft.AspNetCore.Http;

namespace RecipeSchedulerApiService.Types.Inputs
{
    public class IngredientUpdateInput
    {
		public string IngredientName { get; set; }

		public string IngredientDescription { get; set; }

		public float? Calories { get; set; }

		public bool FruitVeg { get; set; }

		public float? Fat { get; set; }

		public float? Salt { get; set; }

		public float? Protein { get; set; }

		public float? Carbs { get; set; }

		public float Quantity { get; set; }

		public string MeasureType { get; set; }
	}
}
