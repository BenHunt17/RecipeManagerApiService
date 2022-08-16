using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Inputs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using RecipeManagerWebApi.Types.DomainObjects;

namespace RecipeManagerWebApi.Controllers
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

        [HttpPut]
        [Route("api/user/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserCredentials userCredentials)
        {
            UserTokens userTokens = await _usersService.Login(userCredentials);

            if(userTokens == null)
            {
                return Unauthorized();
            }

            //Append cookies to be be set in the client browser. This keeps track of the currently signed in user and the refresh token so that the client can request a new bearer token when needed.
            //TODO - same site is none for now since react project is a seperate domain. May wan't to invesitigate this further when it comes to deploymnet
            Response.Cookies.Append("X-User-Name", userCredentials.Username, new CookieOptions() { SameSite = SameSiteMode.None, Secure = true });  //Secure must be true if same site is noneS
            Response.Cookies.Append("X-Refresh-Token", userTokens.RefreshToken, new CookieOptions() { Expires = DateTime.Now.AddDays(7), HttpOnly = true, SameSite = SameSiteMode.None, Secure = true }); //Set to httponly to protect against cross site scripting attacks

            return Ok(userTokens.BearerToken);
        }

        [HttpPut]
        [Route("api/user/logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            //TODO - Investigate how to maybe protect this endpoint from being used to perform DoS attack.
            //       i.e. someone constantly calls this endpoint with someone else's username in their cookie

            if (!Request.Cookies.TryGetValue("X-User-Name", out string username))
            {
                //If there is no user cookie then the server can't know who to logout
                return BadRequest();
            }

            await _usersService.Logout(username); 

            //Appends expired cookies so that the browser deletes them. This is so that the client doesn't send the logged out user / deleted refresh as a cookie anymore.
            Response.Cookies.Delete("X-User-Name", new CookieOptions() { Expires = DateTime.Now, HttpOnly = true, SameSite = SameSiteMode.None, Secure = true }); //Couldn't use the simpler delete method because that doesnt use these settings by default
            Response.Cookies.Delete("X-Refresh-Token", new CookieOptions() { Expires = DateTime.Now, HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });

            return Ok();
        }

        [HttpGet]
        [Route("api/user/refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh()
        {
            //Allows the client to request a new bearer token without logging in. Will ensure that the refresh token is correct for the user. Since the beaer token is returned, a cross site request forgery attack won't be able to do anything
            if (!Request.Cookies.TryGetValue("X-Refresh-Token", out string refreshToken) || !Request.Cookies.TryGetValue("X-User-Name", out string username))
            {
                //If the cookies can't be fetched then there's nothing left to do here
                return BadRequest();
            }

            UserTokens userTokens = await _usersService.Refresh(username, refreshToken);

            if (userTokens == null)
            {
                //Assume that a null token means that they caren't authorised
                return Unauthorized();
            }

            return Ok(userTokens);
        }

        [HttpPost]
        [Route("api/user")]
        public async Task<IActionResult> Create([FromForm] UserCredentials userCredentials)
        {
            return Ok(await _usersService.CreateUser(userCredentials));
        }

        [HttpDelete]
        [Route("api/user/{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            await _usersService.DeleteUser(username);
            return Ok();
        }
    }
}
