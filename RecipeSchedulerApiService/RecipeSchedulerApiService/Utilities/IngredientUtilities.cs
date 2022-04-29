using RecipeSchedulerApiService.Enums;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Utilities
{
    public static class IngredientUtilities
    {
        private const float _weightConstant = 100f; //100g and 100ml are the equivalent real world measurements for a database common unit
        private const float _volumeConstant = 100f;

        private const float _tspDensityConstant = 202.884136f;
        private const float _tbspDensityConstant = 67.628045f;

        //Series of helper methods for converting ingredient quantities to real world measurements and the common unit used in this system. Also deals with quantities at recipe level.

        public static float ConvertQuantityToUnit(float quantity, float density, MeasureType measureType)
        {
            //Converts the basic quantity type units to that of its special measure type units

            switch (measureType)
            {
                case MeasureType.KG:
                    return quantity * _weightConstant;
                case MeasureType.ML:
                    return quantity * _volumeConstant;
                case MeasureType.TSP:
                    return (quantity * _tspDensityConstant) / density;
                case MeasureType.TBSP:
                    return (quantity * _tbspDensityConstant) / density;
                default:
                    return quantity;
            }
        }

        public static float StandardiseAmount(float amount, float currentQuantity, QuantityType quantityType)
        {
            // Converts an amount to the common unit used in the database based on its quantity type and the quantity is has with its initial amount

            switch (quantityType)
            {
                case QuantityType.WEIGHT:
                    return (amount * _weightConstant) / currentQuantity;
                case QuantityType.VOLUME:
                    return (amount * _volumeConstant) / currentQuantity;
                case QuantityType.DISCRETE:
                default:
                    return amount;
                }
        }

        public static float StandardiseQuantity(float currentQuantity, float density, MeasureType measureType)
        {
            //Calculates the standardised quantity using the measure type

            switch (measureType)
            {
                case MeasureType.KG:
                    return currentQuantity / _weightConstant;
                case MeasureType.ML:
                    return (currentQuantity) / _volumeConstant;
                case MeasureType.TSP:
                    return (currentQuantity * density) / _tspDensityConstant;
                case MeasureType.TBSP:
                    return (currentQuantity * density) / _tbspDensityConstant;
                default:
                    return currentQuantity;
            }
        }

        public static void QuantifyIngredients(this RecipeModel recipeModel)
        {
            //Takes a recipe model and changes all of its ingredient's properties to represent themselves as a product of the quantity required for its parent recipe

            foreach (RecipeIngredientModel ingredient in recipeModel.Ingredients)
            {
                ingredient.MeasureType = EnumUtilities.StringToMeasureType(ingredient.MeasureTypeValue);
                ingredient.Calories = ingredient.Calories != null ? ingredient.Quantity * ingredient.Calories : null; //Calculates the exact value using the quantity ratio. If base amount is null then forced to be null too
                ingredient.FruitVeg = ingredient.FruitVeg;
                ingredient.Fat = ingredient.Fat != null ? ingredient.Quantity * ingredient.Fat : null;
                ingredient.Salt = ingredient.Salt != null ? ingredient.Quantity * ingredient.Salt : null;
                ingredient.Protein = ingredient.Protein != null ? ingredient.Quantity * ingredient.Protein : null;
                ingredient.Carbs = ingredient.Carbs != null ? ingredient.Quantity * ingredient.Carbs : null;
                ingredient.Quantity = IngredientUtilities.ConvertQuantityToUnit(ingredient.Quantity, ingredient?.Density ?? 1, ingredient.MeasureType);
            }
        }
    }
}
