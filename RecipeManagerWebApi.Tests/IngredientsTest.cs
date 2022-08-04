using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using RecipeManagerWebApi.Tests.Fixtures;
using RecipeManagerWebApi.Tests.Utilities;
using RecipeSchedulerApiService;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types.Inputs;
using RecipeSchedulerApiService.Utilities;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace RecipeManagerWebApi.Tests
{
    [TestCaseOrderer("RecipeManagerWebApi.Tests.Utilities.PriorityOrderer", "RecipeManagerWebApi.Tests")]
    public class IngredientsTest : IDisposable, IClassFixture<IngredientsTestFixture>
    {
        private TestServer _testServer;
        private HttpClient _testClient;
        private IngredientsTestFixture _ingredientsTestFixture;

        public IngredientsTest(IngredientsTestFixture ingredientsTestFixture)
        {
            _testServer = TestServerBuilder.buildServer();
            _testClient = _testServer.CreateClient();

            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestBearerTokenManager.getBearerToken());

            _ingredientsTestFixture = ingredientsTestFixture; 
        }

        [Fact, TestPriority(0)]
        public async void ShouldCreateIngredient()
        {
            IngredientCreateInput input = new IngredientCreateInput()
            {
                IngredientName = "Integration test ingredient",
                IngredientDescription = "Some description",
                ImageFile = null,
                Calories = 50,
                FruitVeg = true,
                Fat = 10,
                Salt = 20,
                Protein = 30,
                Carbs = 40,
                Quantity = 200,
                MeasureType = "KG"
            };

            IngredientModel expected = new IngredientModel()
            {
                IngredientName = "Integration test ingredient",
                IngredientDescription = "Some description",
                ImageUrl = null,
                Calories = 25,
                FruitVeg = true,
                Fat = 5,
                Salt = 10,
                Protein = 15,
                Carbs = 20,
                MeasureType = "KG",
            };

            HttpRequestMessage request = HttpRequestBuilder.BuildRequest("api/ingredient", HttpMethod.Post).AddFormBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual));

            _ingredientsTestFixture.ingredientModel = actual;
        }

        [Fact, TestPriority(1)]
        public async void ShouldFindIngredient()
        {

            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredientModel.Id}", HttpMethod.Get);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_ingredientsTestFixture.ingredientModel, actual);
        }

        [Fact, TestPriority(1)]
        public async void ShouldUpdateIngredient()
        {
            IngredientUpdateInput input = new IngredientUpdateInput()
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
            };

            IngredientModel expected = new IngredientModel()
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
            };

            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredientModel.Id}", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual));

            _ingredientsTestFixture.ingredientModel = actual;
        }

        [Fact, TestPriority(2)]
        public async void ShouldDeleteIngredient()
        {

            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredientModel.Id}", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_ingredientsTestFixture.ingredientModel, actual);
        }

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}