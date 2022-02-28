using System;
using System.Collections.Generic;

namespace RecipeSchedulerApiService.Models
{
    public class RecipeModel
    {
		public int Id { get; set; }

		public string RecipeName { get; set; }

		public string RecipeDescription { get; set; }

		public string ImageUrl { get; set; }

		public IEnumerable<RecipeIngredientModel> Ingredients { get; set; }

		public IEnumerable<InstructionModel> Instructions { get; set; }

		public int Rating { get; set; }

		public TimeSpan PrepTime { get; set; }

		public int ServingSize { get; set; }

		public bool Breakfast { get; set; }

		public bool Lunch { get; set; }

		public bool Dinner { get; set; }
	}
}
