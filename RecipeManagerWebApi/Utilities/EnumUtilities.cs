using RecipeSchedulerApiService.Enums;

namespace RecipeSchedulerApiService.Utilities
{
    public static class EnumUtilities
    {
        public static MeasureType StringToMeasureType(string measureType)
        {
            switch (measureType)
            {
                case "KG":
                    return MeasureType.KG;
                case "ML":
                    return MeasureType.ML;
                case "DISCRETE":
                    return MeasureType.DISCRETE;
                case "TSP":
                    return MeasureType.TSP;
                case "TBSP":
                    return MeasureType.TBSP;
                default:
                    return MeasureType.NONE;
            }
        }

        public static string MeasureTypeToString(MeasureType measureType)
        {
            switch (measureType)
            {
                case MeasureType.KG:
                    return "KG";
                case MeasureType.ML:
                    return "ML";
                case MeasureType.DISCRETE:
                    return "DISCRETE";
                case MeasureType.TSP:
                    return "TSP";
                case MeasureType.TBSP:
                    return "TBSP";
                default:
                    return "NONE";
            }
        }
    }
}
