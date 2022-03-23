using RecipeSchedulerApiService.Enums;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Utilities;

namespace RecipeSchedulerApiService.Types
{
    public class RecipeIngredient : Ingredient
    {
        public RecipeIngredient(RecipeIngredientModel recipeIngredientModel) : base()
        {
            MeasureType measureType = EnumUtilities.StringToMeasureType(recipeIngredientModel.MeasureTypeValue);

            Id = recipeIngredientModel.Id;
            IngredientName = recipeIngredientModel.IngredientName;
            IngredientDescription = recipeIngredientModel.IngredientDescription;
            ImageUrl = recipeIngredientModel.ImageUrl;
            Calories = (recipeIngredientModel?.Quantity ?? 0) * (recipeIngredientModel?.Calories ?? 0); //Calculates the exact value using the quantity ratio
            FruitVeg = recipeIngredientModel.FruitVeg;
            Fat = (recipeIngredientModel?.Quantity ?? 0) * (recipeIngredientModel?.Fat ?? 0);
            Salt = (recipeIngredientModel?.Quantity ?? 0) * (recipeIngredientModel?.Salt ?? 0);
            Protein = (recipeIngredientModel?.Quantity ?? 0) * (recipeIngredientModel?.Protein ?? 0);
            Carbs = (recipeIngredientModel?.Quantity ?? 0) * (recipeIngredientModel?.Carbs ?? 0);
            Quantity = IngredientUtilities.ConvertQuantityToUnit(recipeIngredientModel?.Quantity ?? 0, recipeIngredientModel?.Density ?? 1, measureType);
            MeasureType = measureType;
        }

        public float Quantity { get; set; }

        public MeasureType MeasureType { get; set; }
    }
}
