using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Controllers
{
    [ApiController]
    [Authorize] //Quest must have valid bearer token for any method on the controller to work
    public class RecipesController : ControllerBase
    {
        private readonly IRecipesService _recipesService;
        private readonly IPropertyFilterInterpreter _propertyFilterInterpreter;

        public RecipesController(IRecipesService recipesService, IPropertyFilterInterpreter propertyFilterInterpreter)
        {
            //Injects the recipes service so that the controller can call its methods
            _recipesService = recipesService;
            _propertyFilterInterpreter = propertyFilterInterpreter;
        }

        [HttpGet]
        [Route("api/recipe/{recipeName}")]
        public async Task<IActionResult> Get(string recipeName)
        {
            return Ok(await _recipesService.GetRecipe(recipeName));
        }

        [HttpGet]
        [Route("api/recipes")]
        public async Task<IActionResult> Get()
        {
            IQueryCollection queryParamters = HttpContext.Request.Query;
            IDictionary<string, List<PropertyFilter>> propertyQueryFilters = _propertyFilterInterpreter.ParsePropertyParameters(queryParamters);

            return Ok(await _recipesService.GetAllRecipes(propertyQueryFilters));
        }

        [HttpPost]
        [Route("api/recipe")]
        public async Task<IActionResult> Create([FromForm] RecipeCreateInput recipeCreateInput)
        {
            IEnumerable<RecipeIngredientInput> recipeIngredientsInput = JsonConvert.DeserializeObject<IEnumerable<RecipeIngredientInput>>(recipeCreateInput.RecipeIngredients);
            IEnumerable<InstructionInput> instructionsInput = JsonConvert.DeserializeObject<IEnumerable<InstructionInput>>(recipeCreateInput.Instructions);

            return Ok(await _recipesService.CreateRecipe(recipeCreateInput, recipeIngredientsInput, instructionsInput));
        }

        [HttpPut]
        [Route("api/recipe/{recipeName}")]
        public async Task<IActionResult> Update(string recipeName, [FromBody] RecipeUpdateInput recipeUpdateInput)
        {
            return Ok(await _recipesService.UpdateRecipe(recipeName, recipeUpdateInput));
        }

        [HttpPut]
        [Route("api/recipe/{recipeName}/recipeingredients")]
        public async Task<IActionResult> UpdateRecipeIngredients(string recipeName, [FromBody] IEnumerable<RecipeIngredientInput> recipeIngredientInputs)
        {
            return Ok(await _recipesService.UpdateRecipeIngredients(recipeName, recipeIngredientInputs));
        }

        [HttpPut]
        [Route("api/recipe/{recipeName}/recipeinstructions")]
        public async Task<IActionResult> UpdateRecipeInstructions(string recipeName, [FromBody] IEnumerable<InstructionInput> instructionInputs)
        {
            return Ok(await _recipesService.UpdateInstructions(recipeName, instructionInputs));
        }

        [HttpPut]
        [Route("api/recipe/{recipeName}/image")]
        public async Task<IActionResult> UploadImage(string recipeName, IFormFile imageFile)
        {
            return Ok(await _recipesService.UploadRecipeImage(recipeName, imageFile));
        }

        [HttpDelete]
        [Route("api/recipe/{recipeName}/image")]
        public async Task<IActionResult> RemoveImage(string recipeName)
        {
            await _recipesService.RemoveRecipeImage(recipeName);
            return NoContent();
        }

        [HttpDelete]
        [Route("api/recipe/{recipeName}")]
        public async Task<IActionResult> Delete(string recipeName)
        {
            await _recipesService.DeleteRecipe(recipeName);
            return NoContent();
        }

    }
}
