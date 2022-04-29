namespace RecipeSchedulerApiService.Types.Inputs
{
    public class RecipeIngredientInput
    {
        public int RecipeIngredientId { get; set; }

        public int Quantity { get; set; }

        public float Density { get; set; }

        public string MeasureTypeValue { get; set; }
    }
}
