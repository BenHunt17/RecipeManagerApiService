using Microsoft.AspNetCore.TestHost;
using RecipeManagerWebApi.Tests.CustomEqualities;
using RecipeManagerWebApi.Tests.IntegrationTests.Ingredients;
using RecipeManagerWebApi.Tests.IntegrationTests.Recipes.TestData;
using RecipeManagerWebApi.Tests.Utilities;
using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Types.Inputs;
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
        public async void ShouldCreateRecipe(RecipeCreateInput input, Recipe expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest("api/recipe", HttpMethod.Post).AddFormBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            Recipe actual = await HttpResponseExtractor.GetObjectResult<Recipe>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Matches(actual));

            _recipesTestFixture.recipe = expected;
        }

        [Fact, TestPriority(1)]
        public async void ShouldFindRecipe()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipe.RecipeName}", HttpMethod.Get);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            Recipe actual = await HttpResponseExtractor.GetObjectResult<Recipe>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(_recipesTestFixture.recipe.Matches(actual));
        }

        [Theory, TestPriority(2)]
        [ClassData(typeof(UpdateRecipeDataGenerator))]
        public async void ShouldUpdateRecipe(RecipeUpdateInput input, Recipe expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipe.RecipeName}", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            Recipe actual = await HttpResponseExtractor.GetObjectResult<Recipe>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Matches(actual));

            _recipesTestFixture.recipe = expected;
        }

        [Theory, TestPriority(3)]
        [ClassData(typeof(RecipeIngredientsDataGenerator))]
        public async void ShouldUpsertIngredients(List<RecipeIngredientInput> input, List<RecipeIngredient> expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipe.RecipeName}/recipeingredients", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            List<RecipeIngredient> actual = await HttpResponseExtractor.GetObjectResult<List<RecipeIngredient>>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.All(ingredient => actual.Any(actualIngredient => ingredient.Matches(actualIngredient))));
        }

        [Theory, TestPriority(3)]
        [ClassData(typeof(InstructionsDataGenerator))]
        public async void ShouldUpsertInstructions(List<InstructionInput> input, List<Instruction> expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipe.RecipeName}/recipeinstructions", HttpMethod.Put).AddJsonBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            List<Instruction> actual = await HttpResponseExtractor.GetObjectResult<List<Instruction>>(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.All(instruction => actual.Any(actualInstruction => instruction.Matches(actualInstruction))));
        }

        [Theory, TestPriority(4)]
        [ClassData(typeof(UploadRecipeImageDataGenerator))]
        public async void ShouldUploadImage(FileStream input, string expected)
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipe.RecipeName}/image", HttpMethod.Put).AddFormBody(input);
            HttpResponseMessage response = await _testClient.SendAsync(request);
            string actual = await HttpResponseExtractor.GetStringResult(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(expected.Equals(actual));
        }

        [Fact, TestPriority(5)]
        public async void ShouldRemoveImage()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipe.RecipeName}/image", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(6)]
        public async void ShouldDeleteRecipe()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/Integration test recipe updated", HttpMethod.Delete);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(7)]
        public async void ShouldNotFindDeletedIngredient()
        {
            HttpRequestMessage request = HttpRequestBuilder.BuildRequest($"api/recipe/{_recipesTestFixture.recipe.RecipeName}", HttpMethod.Get);
            HttpResponseMessage response = await _testClient.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}