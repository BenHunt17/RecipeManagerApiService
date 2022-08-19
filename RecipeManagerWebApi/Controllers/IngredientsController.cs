using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Inputs;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Controllers
{
    [ApiController]
    [Authorize] 
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;

        public IngredientsController(IIngredientsService ingredientsService)
        {
            _ingredientsService = ingredientsService;
        }

        [HttpGet]
        [Route("api/ingredient/{ingredientName}")]
        public async Task<IActionResult> Get(string ingredientName)
        {
            return Ok(await _ingredientsService.GetIngredient(ingredientName));
        }

        [HttpGet]
        [Route("api/ingredients")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _ingredientsService.GetIngredients());
        }

        [HttpPost]
        [Route("api/ingredient")]
        public async Task<IActionResult> Create([FromForm] IngredientCreateInput ingredientCreateInput)
        {
            return Ok(await _ingredientsService.CreateIngredient(ingredientCreateInput));
        }

        [HttpPut]
        [Route("api/ingredient/{ingredientName}")]
        public async Task<IActionResult> Update(string ingredientName, [FromBody] IngredientUpdateInput ingredientUpdateInput)
        {
            return Ok(await _ingredientsService.UpdateIngredient(ingredientName, ingredientUpdateInput));
        }

        [HttpPut]
        [Route("api/ingredient/{ingredientName}/image")]
        public async Task<IActionResult> UploadImage(string ingredientName, IFormFile imageFile)
        {
            return Ok(await _ingredientsService.UploadIngredientImage(ingredientName, imageFile));
        }

        [HttpDelete]
        [Route("api/ingredient/{ingredientName}/image")]
        public async Task<IActionResult> RemoveImage(string ingredientName)
        {
            await _ingredientsService.RemoveIngredientImage(ingredientName);
            return Ok();
        }

        [HttpDelete]
        [Route("api/ingredient/{ingredientName}")]
        public async Task<IActionResult> Delete(string ingredientName)
        {
            await _ingredientsService.DeleteIngredient(ingredientName);
            return Ok();
            //TODO - Look at returning No content status code for endpoints that return no content
        }
    }
}
