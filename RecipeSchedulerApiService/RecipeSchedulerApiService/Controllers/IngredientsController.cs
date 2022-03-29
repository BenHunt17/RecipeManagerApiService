using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulerApiService.Interfaces;
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
    }
}
