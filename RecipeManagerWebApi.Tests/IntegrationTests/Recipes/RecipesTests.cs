using Microsoft.AspNetCore.TestHost;
using RecipeManagerWebApi.Tests.IntegrationTests.Ingredients;
using RecipeManagerWebApi.Tests.IntegrationTests.Recipes.TestData;
using RecipeManagerWebApi.Tests.Utilities;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types.Inputs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace RecipeManagerWebApi.Tests.IntegrationTests.Recipes
{
    [TestCaseOrderer("RecipeManagerWebApi.Tests.Utilities.PriorityOrderer", "RecipeManagerWebApi.Tests")]
    public class RecipesTest : IDisposable, IClassFixture<RecipesTestFixture>
    {
        private TestServer _testServer;
        private HttpClient _testClient;
        private RecipesTestFixture _recipesTestFixture;

        public RecipesTest(RecipesTestFixture recipesTestFixture)
        {
            _testServer = TestServerBuilder.buildServer();
            _testClient = _testServer.CreateClient();

            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestBearerTokenManager.getBearerToken());

            _recipesTestFixture = recipesTestFixture;
        }

        [Theory, TestPriority(0)]
        [ClassData(typeof(CreateRecipeDataGenerator))]
        public async void ShouldCreateRecipe(RecipeCreateInput input, RecipeModel expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest("api/recipe", HttpMethod.Post).AddFormBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            RecipeModel actual = await HttpResponseExtractor.GetObjectResult<RecipeModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual));

            _recipesTestFixture.recipeModel = actual;
        }

        [Fact, TestPriority(1)]
        public async void ShouldFindRecipe()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipeModel.Id}", HttpMethod.Get);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            RecipeModel actual = await HttpResponseExtractor.GetObjectResult<RecipeModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(_recipesTestFixture.recipeModel.Equals(actual));
        }

        [Theory, TestPriority(2)]
        [ClassData(typeof(UpdateRecipeDataGenerator))]
        public async void ShouldUpdateRecipe(RecipeUpdateInput input, RecipeModel expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipeModel.Id}", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            RecipeModel actual = await HttpResponseExtractor.GetObjectResult<RecipeModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual));
        }

        [Theory, TestPriority(3)]
        [ClassData(typeof(RecipeIngredientsDataGenerator))]
        public async void ShouldUpsertIngredients(List<RecipeIngredientInput> input, List<RecipeIngredientModel> expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipeModel.Id}/recipeingredients", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            RecipeModel actual = await HttpResponseExtractor.GetObjectResult<RecipeModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.All(ingredient => actual.Ingredients.Any(actualIngredient => ingredient.Equals(actualIngredient))));
        }

        [Theory, TestPriority(3)]
        [ClassData(typeof(InstructionsDataGenerator))]
        public async void ShouldUpsertInstructions(List<InstructionInput> input, List<InstructionModel> expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipeModel.Id}/recipeinstructions", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            RecipeModel actual = await HttpResponseExtractor.GetObjectResult<RecipeModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.All(instruction => actual.Instructions.Any(actualInstruction => instruction.Equals(actualInstruction))));
        }

        [Theory, TestPriority(4)]
        [ClassData(typeof(UploadRecipeImageDataGenerator))]
        public async void ShouldUploadImage(FileStream input, string expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipeModel.Id}/image", HttpMethod.Put).AddFormBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual.ImageUrl));
        }

        [Fact, TestPriority(5)]
        public async void ShouldRemoveImage()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipeModel.Id}/image", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            IngredientModel actual = await HttpResponseExtractor.GetObjectResult<IngredientModel>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Null(actual.ImageUrl);
        }

        [Fact, TestPriority(6)]
        public async void ShouldDeleteIngredient()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipeModel.Id}", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(7)]
        public async void ShouldNotFindDeletedIngredient()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipeModel.Id}", HttpMethod.Get);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}