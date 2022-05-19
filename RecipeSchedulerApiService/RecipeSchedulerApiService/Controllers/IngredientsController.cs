﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Types.Inputs;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Controllers
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
        [Route("api/ingredient")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _ingredientsService.GetIngredient(id));
        }

        [HttpGet]
        [Route("api/ingredients")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _ingredientsService.GetAllIngredients());
        }

        [HttpPost]
        [Route("api/ingredient")]
        public async Task<IActionResult> Create([FromForm] IngredientCreateInput ingredientCreateInput)
        {
            return Ok(await _ingredientsService.CreateIngredient(ingredientCreateInput));
        }

        [HttpPut]
        [Route("api/ingredient")]
        public async Task<IActionResult> Update(int id, [FromBody] IngredientUpdateInput ingredientUpdateInput)
        {
            return Ok(await _ingredientsService.UpdateIngredient(id, ingredientUpdateInput));
        }

        [HttpPut]
        [Route("api/ingredient/{id}/image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile formFile)
        {
            return Ok(await _ingredientsService.UploadIngredientImage(id, formFile));
        }

        [HttpDelete]
        [Route("api/ingredient/{id}/image")]
        public async Task<IActionResult> RemoveImage(int id)
        {
            return Ok(await _ingredientsService.RemoveIngredientImage(id));
        }

        [HttpDelete]
        [Route("api/ingredient")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _ingredientsService.DeleteIngredient(id));
        }
    }
}