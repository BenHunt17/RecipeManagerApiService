using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Controllers
{
    [ApiController]
    [Authorize] 
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;
        private readonly IPropertyFilterInterpreter _propertyFilterInterpreter;

        public IngredientsController(IIngredientsService ingredientsService, IPropertyFilterInterpreter propertyFilterInterpreter)
        {
            _ingredientsService = ingredientsService;
            _propertyFilterInterpreter = propertyFilterInterpreter;
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
            IQueryCollection queryParamters = HttpContext.Request.Query;
            IDictionary<string, List<PropertyFilter>> propertyQueryFilters = _propertyFilterInterpreter.ParsePropertyParameters(queryParamters);

            return Ok(await _ingredientsService.GetIngredients(propertyQueryFilters));
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
            return NoContent();
        }

        [HttpDelete]
        [Route("api/ingredient/{ingredientName}")]
        public async Task<IActionResult> Delete(string ingredientName)
        {
            await _ingredientsService.DeleteIngredient(ingredientName);
            return NoContent();
        }
    }
}
