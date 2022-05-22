using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Controllers
{
    [ApiController]
    [Authorize] //Quest must have valid bearer token for any method on the controller to work
    public class RecipesController : ControllerBase
    {
        private readonly IRecipesService _recipesService;

        public RecipesController(IRecipesService recipesService)
        {
            //Injects the recipes service so that the controller can call its methods
            _recipesService = recipesService;
        }

        [HttpGet]
        [Route("api/recipe")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _recipesService.GetRecipe(id));
        }

        [HttpGet]
        [Route("api/recipes")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _recipesService.GetAllRecipes());
        }

        [HttpPost]
        [Route("api/recipe")]
        public async Task<IActionResult> Create([FromForm] RecipeCreateInput recipeCreateInput)
        {
            return Ok(await _recipesService.CreateRecipe(recipeCreateInput));
        }

        [HttpPut]
        [Route("api/recipe")]
        public async Task<IActionResult> Update(int id, [FromBody] RecipeUpdateInput recipeUpdateInput)
        {
            return Ok(await _recipesService.UpdateRecipe(id, recipeUpdateInput));
        }

        [HttpPut]
        [Route("api/recipe/{id}/recipeingredients")]
        public async Task<IActionResult> UpdateRecipeIngredients(int id, [FromBody] IEnumerable<RecipeIngredientInput> recipeIngredientInputs)
        {
            return Ok(await _recipesService.UpdateRecipeIngredients(id, recipeIngredientInputs));
        }

        [HttpPut]
        [Route("api/recipe/{id}/recipeinstructions")]
        public async Task<IActionResult> UpdateRecipeInstructions(int id, [FromBody] IEnumerable<InstructionInput> instructionInputs)
        {
            return Ok(await _recipesService.UpdateInstructions(id, instructionInputs));
        }

        [HttpPut]
        [Route("api/recipe/{id}/image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile formFile)
        {
            return Ok(await _recipesService.UploadRecipeImage(id, formFile));
        }

        [HttpDelete]
        [Route("api/recipe/{id}/image")]
        public async Task<IActionResult> RemoveImage(int id)
        {
            return Ok(await _recipesService.RemoveRecipeImage(id));
        }

        [HttpDelete]
        [Route("api/recipe")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _recipesService.DeleteRecipe(id));
        }

    }
}
