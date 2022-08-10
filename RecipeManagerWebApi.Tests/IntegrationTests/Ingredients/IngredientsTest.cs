using Microsoft.AspNetCore.TestHost;
using RecipeManagerWebApi.Tests.IntegrationTests.Ingredients.TestData;
using RecipeManagerWebApi.Tests.Utilities;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types.Inputs;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace RecipeManagerWebApi.Tests.IntegrationTests.Ingredients
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

        [Theory, TestPriority(0)]
        [ClassData(typeof(CreateIngredientDataGenerator))]
        public async void ShouldCreateIngredient(IngredientCreateInput input, IngredientModel expected)
        {
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

        [Theory, TestPriority(2)]
        [ClassData(typeof(UpdateIngredientDataGenerator))]
        public async void ShouldUpdateIngredient(IngredientUpdateInput input, IngredientModel expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredientModel.Id}", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual));
        }

        [Theory, TestPriority(3)]
        [ClassData(typeof(UploadIngredientImageDataGenerator))]
        public async void ShouldUploadImage(FileStream input, string expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredientModel.Id}/image", HttpMethod.Put).AddFormBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual.ImageUrl));
        }

        [Fact, TestPriority(4)]
        public async void ShouldRemoveImage()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredientModel.Id}/image", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Null(actual.ImageUrl);
        }

        [Fact, TestPriority(5)]
        public async void ShouldDeleteIngredient()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredientModel.Id}", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(6)]
        public async void ShouldNotFindDeletedIngredient()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredientModel.Id}", HttpMethod.Get);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}