using Microsoft.AspNetCore.TestHost;
using RecipeManagerWebApi.Tests.CustomEqualities;
using RecipeManagerWebApi.Tests.IntegrationTests.Ingredients.TestData;
using RecipeManagerWebApi.Tests.Utilities;
using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Types.Inputs;
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
        public async void ShouldCreateIngredient(IngredientCreateInput input, Ingredient expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest("api/ingredient", HttpMethod.Post).AddFormBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            Ingredient actual = await HttpResponseExtractor.GetObjectResult<Ingredient>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Matches(actual));

            _ingredientsTestFixture.ingredient = expected;
        }

        [Fact, TestPriority(1)]
        public async void ShouldFindIngredient()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredient.IngredientName}", HttpMethod.Get);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            Ingredient actual = await HttpResponseExtractor.GetObjectResult<Ingredient>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(_ingredientsTestFixture.ingredient.Matches(actual));
        }

        [Theory, TestPriority(2)]
        [ClassData(typeof(UpdateIngredientDataGenerator))]
        public async void ShouldUpdateIngredient(IngredientUpdateInput input, Ingredient expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredient.IngredientName}", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            Ingredient actual = await HttpResponseExtractor.GetObjectResult<Ingredient>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Matches(actual));

            _ingredientsTestFixture.ingredient = expected;
        }

        [Theory, TestPriority(3)]
        [ClassData(typeof(UploadIngredientImageDataGenerator))]
        public async void ShouldUploadImage(FileStream input, string expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredient.IngredientName}/image", HttpMethod.Put).AddFormBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            string actual = await HttpResponseExtractor.GetStringResult(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual));
        }

        [Fact, TestPriority(4)]
        public async void ShouldRemoveImage()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredient.IngredientName}/image", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(5)]
        public async void ShouldDeleteIngredient()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredient.IngredientName}", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(6)]
        public async void ShouldNotFindDeletedIngredient()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/ingredient/{_ingredientsTestFixture.ingredient.IngredientName}", HttpMethod.Get);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}