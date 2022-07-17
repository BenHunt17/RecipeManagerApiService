using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Types.Inputs;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Controllers
{
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("api/user")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserCredentials userCredentials)
        {
            string bearerToken = await _usersService.Login(userCredentials);

            if(bearerToken == null)
            {
                return Unauthorized();
            }
            return Ok(bearerToken);
        }
    }
}
