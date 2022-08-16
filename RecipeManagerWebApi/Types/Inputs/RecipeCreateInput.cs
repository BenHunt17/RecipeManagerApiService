using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Types.Inputs
{
    public class RecipeCreateInput
    {
		public string RecipeName { get; set; }

		public string RecipeDescription { get; set; }

		public IFormFile ImageFile { get; set; }

		public string RecipeIngredients { get; set; }

		public string Instructions { get; set; }

		public int Rating { get; set; }

		public int PrepTime { get; set; }

		public int ServingSize { get; set; }

		public bool Breakfast { get; set; }

		public bool Lunch { get; set; }

		public bool Dinner { get; set; }
	}
}
