namespace RecipeSchedulerApiService.Types.Inputs
{
	public class RecipeUpdateInput
	{
		public string RecipeName { get; set; }

		public string RecipeDescription { get; set; }

		public int Rating { get; set; }

		public int PrepTime { get; set; }

		public int ServingSize { get; set; }

		public bool Breakfast { get; set; }

		public bool Lunch { get; set; }

		public bool Dinner { get; set; }
	}
}
