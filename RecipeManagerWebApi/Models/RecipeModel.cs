using RecipeSchedulerApiService.Types.Inputs;
using System.Collections.Generic;
using System.Linq;

namespace RecipeSchedulerApiService.Models
{
    public class RecipeModel
    {
		public RecipeModel() { }

		public RecipeModel(RecipeCreateInput recipeCreateInput, string imageUrl, IEnumerable<RecipeIngredientInput> recipeIngredientsInput, IEnumerable<InstructionInput> InstructionsInput)
        {
			RecipeName = recipeCreateInput.RecipeName;
			RecipeDescription = recipeCreateInput.RecipeDescription;
			ImageUrl = imageUrl;
			Ingredients = recipeIngredientsInput.ToList().Select(recipeIngredientsInput =>  new RecipeIngredientModel(recipeIngredientsInput));
			Instructions = InstructionsInput.ToList().Select(instruction => new InstructionModel(instruction));
			Rating = recipeCreateInput.Rating;
			PrepTime = recipeCreateInput.PrepTime;
			ServingSize = recipeCreateInput.ServingSize;
			Breakfast = recipeCreateInput.Breakfast;
			Lunch = recipeCreateInput.Lunch;
			Dinner = recipeCreateInput.Dinner;
		}

		public RecipeModel(RecipeUpdateInput recipeUpdateInput)
		{
			RecipeName = recipeUpdateInput.RecipeName;
			RecipeDescription = recipeUpdateInput.RecipeDescription;
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

		public override bool Equals(object obj)
		{
			RecipeModel recipeModel = obj as RecipeModel;

			if (recipeModel == null)
			{
				return false;
			}

			return
				RecipeName == recipeModel.RecipeName &&
				RecipeDescription == recipeModel.RecipeDescription &&
				ImageUrl == recipeModel.ImageUrl &&
				Ingredients.All(ingredient => recipeModel.Ingredients.Any(
					otherIngredient => ingredient.Equals(otherIngredient))) &&
				Instructions.All(instruction => recipeModel.Instructions.Any(
					otherInstruction => instruction.Equals(otherInstruction))) &&
				Rating == recipeModel.Rating &&
				PrepTime == recipeModel.PrepTime &&
				ServingSize == recipeModel.ServingSize &&
				Breakfast == recipeModel.Breakfast &&
				Lunch == recipeModel.Lunch &&
				Dinner == recipeModel.Dinner;
		}
	}
}
