using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RecipeSchedulerApiService.Controllers
{
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("api/token")]
        public IActionResult GetToken(string password)
        {
            //TODO: Need to add some sort of login in the future

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(tokenString);
        }
    }
}
