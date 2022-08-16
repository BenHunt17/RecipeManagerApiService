using RecipeManagerWebApi.Types.Inputs;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManagerWebApi.Types.Models
{
    public class RecipeModel
    {
		public RecipeModel() { }

		public RecipeModel(RecipeCreateInput recipeCreateInput, string imageUrl, IEnumerable<RecipeIngredientModel> recipeIngredientModels, IEnumerable<InstructionModel> InstructionModels)
        {
			RecipeName = recipeCreateInput.RecipeName;
			RecipeDescription = recipeCreateInput.RecipeDescription;
			ImageUrl = imageUrl;
			Ingredients = recipeIngredientModels;
			Instructions = InstructionModels;
			Rating = recipeCreateInput.Rating;
			PrepTime = recipeCreateInput.PrepTime;
			ServingSize = recipeCreateInput.ServingSize;
			Breakfast = recipeCreateInput.Breakfast;
			Lunch = recipeCreateInput.Lunch;
			Dinner = recipeCreateInput.Dinner;
		}

		public RecipeModel(RecipeUpdateInput recipeUpdateInput, RecipeModel existingRecipeModel)
		{
			RecipeName = recipeUpdateInput.RecipeName;
			RecipeDescription = recipeUpdateInput.RecipeDescription;
			ImageUrl = existingRecipeModel.ImageUrl;
			Ingredients = existingRecipeModel.Ingredients;
			Instructions = existingRecipeModel.Instructions;
			Rating = recipeUpdateInput.Rating;
			PrepTime = recipeUpdateInput.PrepTime;
			ServingSize = recipeUpdateInput.ServingSize;
			Breakfast = recipeUpdateInput.Breakfast;
			Lunch = recipeUpdateInput.Lunch;
			Dinner = recipeUpdateInput.Dinner;
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
