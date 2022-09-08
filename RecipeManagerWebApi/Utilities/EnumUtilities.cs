using RecipeManagerWebApi.Enums;
using System;

namespace RecipeManagerWebApi.Utilities
{
    public static class EnumUtilities
    {
        public static MeasureType ExtractMeasureType(this int measureTypeId)
        {
            if(!Enum.IsDefined(typeof(MeasureType), measureTypeId))
            {
                return MeasureType.NONE;
            }
            
            return (MeasureType)measureTypeId;
        }

        public static MeasureType StringToMeasureType(this string measureType)
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

        public static string ToMeasureTypeString(this MeasureType measureType)
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

        public static PropertyFilterOperationType ToFilterOperationType(this string propertyFilterOperationType)
        {
            switch (propertyFilterOperationType)
            {
                case "EQ":
                    return PropertyFilterOperationType.EQ;
                case "NEQ":
                    return PropertyFilterOperationType.NEQ;
                case "GT":
                    return PropertyFilterOperationType.GT;
                case "LT":
                    return PropertyFilterOperationType.LT;
                case "LTE":
                    return PropertyFilterOperationType.LTE;
                case "GTE":
                    return PropertyFilterOperationType.GTE;
                case "LIKE":
                    return PropertyFilterOperationType.LIKE;
                case "PAGE":
                    return PropertyFilterOperationType.PAGE;
                default:
                    return PropertyFilterOperationType.NONE;
            }
        }
    }
}
