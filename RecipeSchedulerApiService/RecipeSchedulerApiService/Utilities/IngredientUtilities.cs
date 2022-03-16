using RecipeSchedulerApiService.Enums;

namespace RecipeSchedulerApiService.Utilities
{
    public static class IngredientUtilities
    {
        private const float _weightConstant = 500f;
        private const float _volumeConstant = 500f;

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
    }
}
