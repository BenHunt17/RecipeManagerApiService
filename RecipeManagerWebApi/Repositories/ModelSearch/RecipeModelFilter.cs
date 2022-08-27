namespace RecipeManagerWebApi.Repositories.ModelSearch
{
    public class RecipeModelFilter
    {
        public string RecipeNameSnippet { get; set; }
		
		public int MinRating { get; set; }

		public int MaxRating { get; set; }

		public int MinPrepTime { get; set; }

		public int MaxPrepTime { get; set; }

		public int MinServingSize { get; set; }

		public int MaxServingSize { get; set; }

		public bool Breakfast { get; set; }

		public bool Lunch { get; set; }

		public bool Dinner { get; set; }
	}
}
