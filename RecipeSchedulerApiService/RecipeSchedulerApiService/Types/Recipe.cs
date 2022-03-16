using RecipeSchedulerApiService.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeSchedulerApiService.Types
{
    public class Recipe
    {
		public Recipe(RecipeModel recipeModel)
        {
			//Constructor which takes the recipe model as an argument and uses its values to create a new recipe type

			Id = recipeModel.Id;
			RecipeName = recipeModel.RecipeName;
			ImageUrl = recipeModel.ImageUrl;
			Ingredients = recipeModel.Ingredients.ToList().Select(ingredient => new RecipeIngredient(ingredient));
			Instructions = recipeModel.Instructions.ToList().Select(instruction => new Instruction(instruction));
			Rating = recipeModel.Rating;
			PrepTime = recipeModel.PrepTime;
			ServingSize = recipeModel.ServingSize;
			Breakfast = recipeModel.Breakfast;
			Lunch = recipeModel.Lunch;
			Dinner = recipeModel.Dinner;
		}

		public int Id { get; set; }

		public string RecipeName { get; set; }

		public string RecipeDescription { get; set; }

		public string ImageUrl { get; set; }

		public IEnumerable<RecipeIngredient> Ingredients { get; set; }

		public IEnumerable<Instruction> Instructions { get; set; }

		public int Rating { get; set; }

		public int PrepTime { get; set; }

		public int ServingSize { get; set; }

		public bool Breakfast { get; set; }

		public bool Lunch { get; set; }

		public bool Dinner { get; set; }
	}
}
