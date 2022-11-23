using RecipeManagerWebApi.Enums;
using System;

namespace RecipeManagerWebApi.Utilities
{
    public static class EnumUtilities
    {
        public static MeasureUnit ExtractMeasureUnit(this int measureUnitId)
        {
            if(!Enum.IsDefined(typeof(MeasureUnit), measureUnitId))
            {
                return MeasureUnit.NONE;
            }
            
            return (MeasureUnit)measureUnitId;
        }

        public static MeasureUnit StringToMeasureUnit(this string measureUnit)
        {
            switch (measureUnit)
            {
                case "G":
                    return MeasureUnit.G;
                case "ML":
                    return MeasureUnit.ML;
                case "DISCRETE":
                    return MeasureUnit.DISCRETE;
                case "TSP":
                    return MeasureUnit.TSP;
                case "TBSP":
                    return MeasureUnit.TBSP;
                default:
                    return MeasureUnit.NONE;
            }
        }

        public static string ToMeasureUnitString(this MeasureUnit measureUnit)
        {
            switch (measureUnit)
            {
                case MeasureUnit.G:
                    return "G";
                case MeasureUnit.ML:
                    return "ML";
                case MeasureUnit.DISCRETE:
                    return "DISCRETE";
                case MeasureUnit.TSP:
                    return "TSP";
                case MeasureUnit.TBSP:
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
