using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Types
{
    public class RecipeListItem
    {
        //Recipe list item type with less data than normal recipe type

        public RecipeListItem(RecipeModel recipeModel)
        {
            Id = recipeModel.Id;
            RecipeName = recipeModel.RecipeName;
            ImageUrl = recipeModel.ImageUrl;
            PrepTime = recipeModel.PrepTime;
            Rating = recipeModel.Rating;
        }

        public int Id { get; set; }

        public string RecipeName { get; set; }

        public string ImageUrl { get; set; }

        public int PrepTime { get; set; } 

        public int Rating { get; set; } 
    }
}
