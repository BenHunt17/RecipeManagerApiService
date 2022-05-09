namespace RecipeSchedulerApiService.Types.Inputs
{
    public class RecipeIngredientInput
    {
        public int RecipeIngredientId { get; set; }

        public float Quantity { get; set; }

        public string MeasureTypeValue { get; set; }
    }
}
