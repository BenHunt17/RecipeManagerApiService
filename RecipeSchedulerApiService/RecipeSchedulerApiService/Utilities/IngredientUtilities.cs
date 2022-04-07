using RecipeSchedulerApiService.Enums;

namespace RecipeSchedulerApiService.Utilities
{
    public static class IngredientUtilities
    {
        private const float _weightConstant = 100f; //100g and 100ml are the equivalent real world measurements for a database common unit
        private const float _volumeConstant = 100f;

        private const float _tspDensityConstant = 202.884136f;
        private const float _tbspDensityConstant = 67.628045f;

        //Series of helper methods for crunching the stats of ingredients to fit the context of which recipe it is tied to

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
                    return (amount * currentQuantity) / _weightConstant;
                case QuantityType.VOLUME:
                    return (amount * currentQuantity) / _volumeConstant;
                case QuantityType.DISCRETE:
                default:
                    return amount;
                }
        }

        public static float StandardiseQuantity(float currentQuantity, float density, MeasureType measureType)
        {
            //Calculates the standardised quantity using the measure type

            //Unused for now but will be used for recipe ingredients

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
    }
}
