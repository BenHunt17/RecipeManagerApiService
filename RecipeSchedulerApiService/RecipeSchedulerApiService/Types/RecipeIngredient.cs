using RecipeSchedulerApiService.Enums;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Utilities;

namespace RecipeSchedulerApiService.Types
{
    public class RecipeIngredient
    {
        public RecipeIngredient(RecipeIngredientModel recipeIngredientModel) 
        {
            MeasureType measureType = EnumUtilities.StringToMeasureType(recipeIngredientModel.MeasureTypeValue);

            Id = recipeIngredientModel.Id;
            IngredientName = recipeIngredientModel.IngredientName;
            Calories = recipeIngredientModel.Calories != null ? recipeIngredientModel.Quantity * recipeIngredientModel.Calories : null; //Calculates the exact value using the quantity ratio. If base amount is null then forced to be null too
            FruitVeg = recipeIngredientModel.FruitVeg;
            Fat = recipeIngredientModel.Fat != null ? recipeIngredientModel.Quantity * recipeIngredientModel.Fat : null;
            Salt = recipeIngredientModel.Salt != null ? recipeIngredientModel.Quantity * recipeIngredientModel.Salt : null;
            Protein = recipeIngredientModel.Protein != null ? recipeIngredientModel.Quantity * recipeIngredientModel.Protein : null;
            Carbs = recipeIngredientModel.Carbs != null ? recipeIngredientModel.Quantity * recipeIngredientModel.Carbs : null;
            Quantity = IngredientUtilities.ConvertQuantityToUnit(recipeIngredientModel.Quantity, recipeIngredientModel?.Density ?? 1, measureType);
            MeasureType = measureType;
        }

        public int Id { get; set; }

        public string IngredientName { get; set; }

        public float? Density { get; set; }

        public float? Calories { get; set; }

        public bool FruitVeg { get; set; }

        public float? Fat { get; set; }

        public float? Salt { get; set; }

        public float? Protein { get; set; }

        public float? Carbs { get; set; }

        public float Quantity { get; set; }

        public MeasureType MeasureType { get; set; }
    }
}
