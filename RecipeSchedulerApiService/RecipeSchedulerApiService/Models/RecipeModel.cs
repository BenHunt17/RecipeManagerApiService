using RecipeSchedulerApiService.Types.Inputs;
using RecipeSchedulerApiService.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace RecipeSchedulerApiService.Models
{
    public class RecipeModel
    {
		public RecipeModel() { }

		public RecipeModel(RecipeCreateInput recipeCreateInput, string imageUrl)
        {
			RecipeName = recipeCreateInput.RecipeName;
			RecipeDescription = recipeCreateInput.RecipeDescription;
			ImageUrl = imageUrl;
			Ingredients = recipeCreateInput.RecipeIngredients.ToList().Select(recipeIngredient =>  //Converts the recipe ingredients input into the recipe ingredient model type while also standardising the quantities with respect to the measure type
				new RecipeIngredientModel()
				{
					Id = recipeIngredient.RecipeIngredientId,
					Quantity = IngredientUtilities.StandardiseQuantity(recipeIngredient.Quantity, recipeIngredient.Density, EnumUtilities.StringToMeasureType(recipeIngredient.MeasureTypeValue)),
					MeasureTypeValue = recipeIngredient.MeasureTypeValue
				});
			Instructions = recipeCreateInput.Instructions.ToList().Select(instruction => new InstructionModel(instruction));
			Rating = recipeCreateInput.Rating;
			PrepTime = recipeCreateInput.PrepTime;
			ServingSize = recipeCreateInput.ServingSize;
			Breakfast = recipeCreateInput.Breakfast;
			Lunch = recipeCreateInput.Lunch;
			Dinner = recipeCreateInput.Dinner;
		}

		public int Id { get; set; }

		public string RecipeName { get; set; }

		public string RecipeDescription { get; set; }

		public string ImageUrl { get; set; }

		public IEnumerable<RecipeIngredientModel> Ingredients { get; set; }

		public IEnumerable<InstructionModel> Instructions { get; set; }

		public int Rating { get; set; }

		public int PrepTime { get; set; }

		public int ServingSize { get; set; }

		public bool Breakfast { get; set; }

		public bool Lunch { get; set; }

		public bool Dinner { get; set; }
	}
}
