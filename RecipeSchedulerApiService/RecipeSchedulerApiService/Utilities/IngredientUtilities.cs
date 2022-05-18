using RecipeSchedulerApiService.Enums;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Utilities
{
    //TODO- Document these methods better
    public static class IngredientUtilities
    {
        private const float _weightConstant = 100f; //100g and 100ml are the equivalent real world measurements for a database common unit
        private const float _volumeConstant = 100f;

        private const float _tspDensityConstant = 202.884136f;
        private const float _tbspDensityConstant = 67.628045f;

        //Series of helper methods for converting ingredient quantities to real world measurements and the common unit used in this system. Also deals with quantities at recipe level.

        public static float ConvertQuantityToRealWorldQuantity(float quantity, float density, MeasureType measureType)
        {
            //Converts a quantity (assumed to be of the common database unit) to its real world measure type
            //Basically maps a database normalised quantity value to a real world quantity value (Used for recipe ingredients on recipe)

            if(density == 0)
            {
                //Gaurds against a divide by 0 
                return 0;
            }

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

        public static float StandardiseRealWorldQuantity(float currentQuantity, float density, MeasureType measureType)
        {
            //Scales a quantity with respect to the ration of the user defined quanitity to the common real world unit value
            //CurrentQuantity is the quantity of the recipe ingredient which may not be the common unit value.
            //Density of the ingredient is needed for converting quantities for some measure types.
            //MeasureType is measure type
            //Basically maps a real world quantity to a database normalised quantity (Used when creating a recipe with recipe ingredients which have differing quantities)

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

        public static float ConvertQuantityToRealWorldQuantity(float statistic, float quanitity)
        {
            //Scales a statistic to a real world measurement using a specific quantity
            //Basically just gives the product of the quantity and statistic so that the statistic reflects the specific amount of an ingredient and not just some normalised value

            return statistic * quanitity;
        }

        public static float StandardiseRealWorldStatistic(float statistic, float currentQuantity, QuantityType quantityType)
        {
            //Scales a statistic with respect to the ratio of the common unit real world value to a user defined quantity
            //Amount is the actual amount of something eg. 10g protein
            //CurrentQuantity is the quanity which the user defined. For example, 200g. Using the amount example, this would be interpreted as "200g of ingredient X contains 10g protein"
            //QuantityType is simply the typee which determines if and how the scaling happens. In essence the common unit quantity is divided by the current quantity to give a scale factor
            //Basically maps a real world statistic to a database one (used for creating ingredients with properties when the user may have used different quantities as a reference)

            if(currentQuantity == 0)
            {
                return 0;
            }

            switch (quantityType)
            {
                case QuantityType.WEIGHT:
                    return (statistic * _weightConstant) / currentQuantity;
                case QuantityType.VOLUME:
                    return (statistic * _volumeConstant) / currentQuantity;
                default:
                    return statistic;
                }
        }

        public static void StandardiseIngredientStatistics(IngredientModel ingredientModel, float quantity)
        {
            // Standardises all real world amounts of relevant statistics to database units in an ingredient model.

            if (ingredientModel.Calories != null)
            {
                ingredientModel.Calories = StandardiseRealWorldStatistic((float)ingredientModel.Calories, quantity, EnumUtilities.StringToQuantityType(ingredientModel.QuantityTypeValue)); 
            }
            if(ingredientModel.Fat != null)
            {
                ingredientModel.Fat = StandardiseRealWorldStatistic((float)ingredientModel.Fat, quantity, EnumUtilities.StringToQuantityType(ingredientModel.QuantityTypeValue));
            }
            if(ingredientModel.Salt != null)
            {
                ingredientModel.Salt = StandardiseRealWorldStatistic((float)ingredientModel.Salt, quantity, EnumUtilities.StringToQuantityType(ingredientModel.QuantityTypeValue));
            }
            if(ingredientModel.Protein != null)
            {
                ingredientModel.Protein = StandardiseRealWorldStatistic((float)ingredientModel.Protein, quantity, EnumUtilities.StringToQuantityType(ingredientModel.QuantityTypeValue));
            }
            if(ingredientModel.Carbs != null)
            {
                ingredientModel.Carbs = StandardiseRealWorldStatistic((float)ingredientModel.Carbs, quantity, EnumUtilities.StringToQuantityType(ingredientModel.QuantityTypeValue));
            }
        }

        public static void QuantifyIngredients(this RecipeModel recipeModel)
        {
            // Converts all recipe ingredient statistics from a recipe model to a real world quantity

            foreach (RecipeIngredientModel ingredient in recipeModel.Ingredients)
            {
                ingredient.MeasureType = EnumUtilities.StringToMeasureType(ingredient.MeasureTypeValue);

                if (ingredient.Calories != null)
                {
                    ingredient.Calories = ConvertQuantityToRealWorldQuantity((float)ingredient.Calories, ingredient.Quantity);
                }
                if (ingredient.Fat != null)
                {
                    ingredient.Fat = ConvertQuantityToRealWorldQuantity((float)ingredient.Fat, ingredient.Quantity);
                }
                if (ingredient.Salt != null)
                {
                    ingredient.Salt = ConvertQuantityToRealWorldQuantity((float)ingredient.Salt, ingredient.Quantity);
                }
                if (ingredient.Protein != null)
                {
                    ingredient.Protein = ConvertQuantityToRealWorldQuantity((float)ingredient.Protein, ingredient.Quantity);
                }
                if (ingredient.Carbs != null)
                {
                    ingredient.Carbs = ConvertQuantityToRealWorldQuantity((float)ingredient.Carbs, ingredient.Quantity);
                }
                if (ingredient.Calories != null)
                {
                    ingredient.Calories = ConvertQuantityToRealWorldQuantity((float)ingredient.Calories, ingredient.Quantity);
                }

                ingredient.Quantity = ConvertQuantityToRealWorldQuantity(ingredient.Quantity, ingredient?.Density ?? 1, ingredient.MeasureType);
            }
        }
    }
}
