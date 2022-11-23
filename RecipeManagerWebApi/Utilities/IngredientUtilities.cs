using System;

namespace RecipeManagerWebApi.Utilities
{
    public static class IngredientUtilities
    {
        private const float _tolerance = 0.0001f;

        //Series of helper methods for converting ingredient quantities to real world measurements and the common unit used in this system. Also deals with quantities at recipe level.

        public static float? StandardiseIngredientStatistic(this float? ingredientStatistic, float currentQuantity)
        {
            //Standardises an ingredient to correspond with the common unit of quantity

            if (ingredientStatistic is float statistic)
            { 
                return statistic / currentQuantity;
            }

            return null;
        }

        public static float? ScaleIngredientStatistic(this float? ingredientStatistic, float currentQuantity)
        {
            //scales a statistic based on the quantity

            if (ingredientStatistic is float statistic)
            {
                return statistic * currentQuantity;
            }

            return null;
        }

        public static bool ApproxEquals(this float? statistic, float? otherStatistic)
        {
            //Since comparing floats for equality is a bad idea due to rounding errors, A helper method for approxEquals is needed.
            //There doesn't seem to be a silver bullet approach for this so I put it in the ingredient utils class since this is specifically used to compare ingredient stats in tests

            //There are some clever things thaat can be done but considering the values used in these tests shouldn't be really big, an arbritary small number will do as a tolerance

            if (statistic == null || otherStatistic == null)
            {
                //If either are null then they should be equal.
                return statistic == otherStatistic;
            }

            if (statistic == otherStatistic)
            {
                return true;
            }

            float difference = Math.Abs((float)statistic - (float)otherStatistic); //Null check above makes this casting safe

            if (difference <= _tolerance)
            {
                return true;
            }

            return false;
        }
    }
}
