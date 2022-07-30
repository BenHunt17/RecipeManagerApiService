using RecipeSchedulerApiService.Enums;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Types
{
    public class IngredientListItem
    {
        //Essentially the ingredient type but with less data. Front end only needs to know some of the data since it is more interested in the ingredients as a collection. Therefore this saves time and money when returning the data.
        public IngredientListItem(IngredientModel ingredientModel)
        {
            Id = ingredientModel.Id;
            IngredientName = ingredientModel.IngredientName;
            ImageUrl = ingredientModel.ImageUrl;
            FruitVeg = ingredientModel.FruitVeg;
            MeasureType = ingredientModel.MeasureType;
        }

        public int Id { get; set; }

        public string IngredientName { get; set; }

        public string ImageUrl { get; set; }

        public bool FruitVeg { get; set; } //May be useful to see if ingredient is healthy at a glance

        public string MeasureType { get; set; }
    }
}
