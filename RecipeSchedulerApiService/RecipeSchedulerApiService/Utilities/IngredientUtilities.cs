using RecipeSchedulerApiService.Enums;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Utilities
{
    public static class IngredientUtilities
    {
        private const float _defaultKgQuantity = 100f; //100g and 100ml are the equivalent real world measurements for a database common unit
        private const float _defaultMlQuantity = 100f;
        private const float _defaultDiscreteQuantity = 1f;
        private const float _defaultTspQuantity = 1f;
        private const float _defaultTbspConstant = 1f;

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
                    return currentQuantity;
            }
        }

        public static void StandardiseIngredientStatistics(this IngredientModel ingredientModel, float currentQuantity)
        {
            //Takes an ingredient model in the form a user provided along with the quantity they used. Standardises the nutrional stats based on how the user defined quantity compares to the default.

            float standardisedQuantity = StandardiseIngredientQuantity(currentQuantity, EnumUtilities.StringToMeasureType(ingredientModel.MeasureTypeValue));

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

            if (recipeIngredientModel.Calories != null)
            {
                recipeIngredientModel.Calories *= recipeIngredientModel.Quantity;
            }
            if (recipeIngredientModel.Fat != null)
            {
                recipeIngredientModel.Fat  *= recipeIngredientModel.Quantity;
            }
            if (recipeIngredientModel.Salt != null)
            {
                recipeIngredientModel.Salt *= recipeIngredientModel.Quantity;
            }
            if (recipeIngredientModel.Protein != null)
            {
                recipeIngredientModel.Protein *= recipeIngredientModel.Quantity;
            }
            if (recipeIngredientModel.Carbs != null)
            {
                recipeIngredientModel.Carbs *= recipeIngredientModel.Quantity;
            }
        }
    }
}
