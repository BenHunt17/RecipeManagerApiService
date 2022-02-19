using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RecipeSchedulerApiService.Controllers
{
    [ApiController]
    public class RecipesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("api/recipes")]
        public IActionResult Get()
        {
            return Ok(new List<string> { "beans on toast", "bangers and mash", "porridge" });
        }
    }
}
