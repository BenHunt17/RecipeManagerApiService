using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types.Inputs;
using System.Collections;
using System.Collections.Generic;

namespace RecipeManagerWebApi.Tests.IntegrationTests.Ingredients
{
    //Instead of cluttering up the test suite with all of the update data objects, Have just defined them here in this class which implements an IEnumerable of object arrays.

    public class UpdateIngredientDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>()
        {
            new object[]
            {
                new IngredientUpdateInput()
                {
                    IngredientName = "Integration test ingredient Updated",
                    IngredientDescription = "Some description updated",
                    Calories = 140,
                    FruitVeg = false,
                    Fat = 120,
                    Salt = 100,
                    Protein = 80,
                    Carbs = 60,
                    Quantity = 400,
                    MeasureType = "KG"
                },
                new IngredientModel()
                {
                    IngredientName = "Integration test ingredient Updated",
                    IngredientDescription = "Some description updated",
                    ImageUrl = null,
                    Calories = 35,
                    FruitVeg = false,
                    Fat = 30,
                    Salt = 25,
                    Protein = 20,
                    Carbs = 15,
                    MeasureType = "KG",
                }
            },
            new object[]
            {
                new IngredientUpdateInput()
                {
                    IngredientName = "Integration test ingredient Updated",
                    IngredientDescription = "Some description updated",
                    Calories = 80.84f,
                    FruitVeg = false,
                    Fat = 0,
                    Salt = 4.83847757372f,
                    Protein = 0.1f,
                    Carbs = 0.000001f,
                    Quantity = 500,
                    MeasureType = "ML"
                },
                new IngredientModel()
                {
                    IngredientName = "Integration test ingredient Updated",
                    IngredientDescription = "Some description updated",
                    ImageUrl = null,
                    Calories = 16.168f,
                    FruitVeg = false,
                    Fat = 0,
                    Salt = 0.96769551474f,
                    Protein = 0.02f,
                    Carbs = 0.0000002f,
                    MeasureType = "ML",
                }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() 
        { 
            return _data.GetEnumerator();
        }
    }
}
