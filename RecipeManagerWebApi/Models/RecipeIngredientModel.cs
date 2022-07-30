﻿using RecipeSchedulerApiService.Types.Inputs;

namespace RecipeSchedulerApiService.Models
{
    public class RecipeIngredientModel : IngredientModel
    {
        public RecipeIngredientModel () { }

        public RecipeIngredientModel(RecipeIngredientInput recipeIngredientInput)
        {
            IngredientId = recipeIngredientInput.IngredientId;
            Quantity = recipeIngredientInput.Quantity;
        }

        public int IngredientId { get; set; }

		public float Quantity { get; set; }

        public bool CompareInput(RecipeIngredientModel recipeIngredientModel)
        {
            //This method takes in a recipe ingredient model which is assumed to have a lot of null fields due to it being mapped using the ingredient input constructor above.
            //This method just checks the important fields and returns whether it's the same recipe ingredient

            return IngredientId == recipeIngredientModel.IngredientId && Quantity == recipeIngredientModel.Quantity;
        }
    }
}
