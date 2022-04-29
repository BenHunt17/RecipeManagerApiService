using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Types.Inputs;
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

    }
}
