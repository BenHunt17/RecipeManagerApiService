using RecipeManagerWebApi.Types.Models;
using RecipeManagerWebApi.Utilities;

namespace RecipeManagerWebApi.Types
{
    public class IngredientListItem
    {
        //Essentially the ingredient type but with less data. It is assumed that whatever is consuming this API doesn't need detailed info on every ingredient when querying them as a collection.
        //This should save time and money.

        public IngredientListItem(IngredientModel ingredientModel)
        {
            IngredientName = ingredientModel.IngredientName;
            ImageUrl = ingredientModel.ImageUrl;
            FruitVeg = ingredientModel.FruitVeg;
            MeasureUnit = ingredientModel.MeasureUnitId.ExtractMeasureUnit().ToMeasureUnitString();
        }

        public string IngredientName { get; set; }

        public string ImageUrl { get; set; }

        public bool FruitVeg { get; set; } 

        public string MeasureUnit { get; set; }
    }
}
