using RecipeSchedulerApiService.Enums;
using RecipeSchedulerApiService.Models;
using System;

namespace RecipeSchedulerApiService.Utilities
{
    public static class IngredientUtilities
    {
        private const float _defaultKgQuantity = 100f; //100g and 100ml are the equivalent real world measurements for a database common unit
        private const float _defaultMlQuantity = 100f;
        private const float _defaultDiscreteQuantity = 1f;
        private const float _defaultTspQuantity = 1f;
        private const float _defaultTbspConstant = 1f;

        private const float _tolerance = 0.0001f;

        //Series of helper methods for converting ingredient quantities to real world measurements and the common unit used in this system. Also deals with quantities at recipe level.

        public static float StandardiseIngredientQuantity(float currentQuantity, MeasureType measureType)
        {
            //Calculates an ingredient's "true" quantity as a fraction of its measure type's "default" quantity. 

            switch (measureType)
            {
                case MeasureType.KG:
                    return _defaultKgQuantity / currentQuantity;
                case MeasureType.ML:
                    return _defaultMlQuantity / currentQuantity;
                case MeasureType.DISCRETE:
                    return _defaultDiscreteQuantity / currentQuantity;
                case MeasureType.TSP:
                    return _defaultTspQuantity / currentQuantity;
                case MeasureType.TBSP:
                    return _defaultTbspConstant / currentQuantity;
                default:
                    return 1;
            }
        }

        public static void StandardiseIngredientStatistics(this IngredientModel ingredientModel, float currentQuantity)
        {
            //Takes an ingredient model in the form a user provided along with the quantity they used. Standardises the nutrional stats based on how the user defined quantity compares to the default.

            float standardisedQuantity = StandardiseIngredientQuantity(currentQuantity, EnumUtilities.StringToMeasureType(ingredientModel.MeasureType));

            if (ingredientModel.Calories != null)
            {
                ingredientModel.Calories *= standardisedQuantity;
            }
            if (ingredientModel.Fat != null)
            {
                ingredientModel.Fat *= standardisedQuantity;
            }
            if (ingredientModel.Salt != null)
            {
                ingredientModel.Salt *= standardisedQuantity;
            }
            if (ingredientModel.Protein != null)
            {
                ingredientModel.Protein *= standardisedQuantity;
            }
            if (ingredientModel.Carbs != null)
            {
                ingredientModel.Carbs *= standardisedQuantity;
            }
        }

        public static void ScaleRecipeIngredientStatistics(this RecipeIngredientModel recipeIngredientModel)
        {
            // Takes a recipe ingredient model and scales its nutrional stats based on the quantity of the recipe ingredient.

            float standardisedQuantity = StandardiseIngredientQuantity(recipeIngredientModel.Quantity, EnumUtilities.StringToMeasureType(recipeIngredientModel.MeasureType));

            if (recipeIngredientModel.Calories != null)
            {
                recipeIngredientModel.Calories /= standardisedQuantity;
            }
            if (recipeIngredientModel.Fat != null)
            {
                recipeIngredientModel.Fat /= standardisedQuantity;
            }
            if (recipeIngredientModel.Salt != null)
            {
                recipeIngredientModel.Salt /= standardisedQuantity;
            }
            if (recipeIngredientModel.Protein != null)
            {
                recipeIngredientModel.Protein /= standardisedQuantity;
            }
            if (recipeIngredientModel.Carbs != null)
            {
                recipeIngredientModel.Carbs /= standardisedQuantity;
            }
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
