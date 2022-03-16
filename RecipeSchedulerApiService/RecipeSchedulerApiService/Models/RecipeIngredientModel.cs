using RecipeSchedulerApiService.Enums;

namespace RecipeSchedulerApiService.Models
{
    public class RecipeIngredientModel : IngredientModel
    {
		public float Quantity { get; set; }

        public string MeasureTypeValue { get; set; }
    }
}

