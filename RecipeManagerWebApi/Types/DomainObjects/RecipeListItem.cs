

using RecipeManagerWebApi.Types.Models;

namespace RecipeManagerWebApi.Types.DomainObjects
{
    public class RecipeListItem
    {
        //Recipe list item type with less data than the normal recipe type

        public RecipeListItem(RecipeModel recipeModel)
        {
            RecipeName = recipeModel.RecipeName;
            ImageUrl = recipeModel.ImageUrl;
            PrepTime = recipeModel.PrepTime;
            Rating = recipeModel.Rating;
        }

        public string RecipeName { get; set; }

        public string ImageUrl { get; set; }

        public int PrepTime { get; set; } 

        public int Rating { get; set; } 
    }
}
