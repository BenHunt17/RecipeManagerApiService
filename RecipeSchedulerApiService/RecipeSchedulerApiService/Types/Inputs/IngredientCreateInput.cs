using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulerApiService.Enums;

namespace RecipeSchedulerApiService.Types.Inputs
{
    public class IngredientCreateInput
    {
		public string IngredientName { get; set; }

		public string IngredientDescription { get; set; }

		public IFormFile ImageFile { get; set; }

		public float? Density { get; set; }

		public float? Calories { get; set; }

		public bool FruitVeg { get; set; }

		public float? Fat { get; set; }

		public float? Salt { get; set; }

		public float? Protein { get; set; }

		public float? Carbs { get; set; }

		public float Quantity { get; set; }

		public string QuantityType { get; set; }
	}
}
