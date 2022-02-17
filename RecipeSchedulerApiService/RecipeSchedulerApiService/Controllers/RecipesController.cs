using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController : ControllerBase
    {
        [HttpGet]
        [Route("api/recipes")]
        public IActionResult Get()
        {
            return Ok(new List<string> { "beans on toast", "bangers and mash", "porridge" });
        }
    }
}
