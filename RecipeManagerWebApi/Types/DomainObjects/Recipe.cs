using RecipeManagerWebApi.Types.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManagerWebApi.Types.DomainObjects
{
	public class Recipe
	{
		public Recipe() { }

		public Recipe(RecipeModel recipeModel, IEnumerable<RecipeIngredient> recipeIngredients)
		{
			RecipeName = recipeModel.RecipeName;
			RecipeDescription = recipeModel.RecipeDescription;
			ImageUrl = recipeModel.ImageUrl;
			Ingredients = recipeIngredients;
			Instructions = recipeModel.Instructions.Select(instruction => new Instruction(instruction));
			Rating = recipeModel.Rating;
			PrepTime = recipeModel.PrepTime;
			ServingSize = recipeModel.ServingSize;
			Breakfast = recipeModel.Breakfast;
			Lunch = recipeModel.Lunch;
			Dinner = recipeModel.Dinner;
		}

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