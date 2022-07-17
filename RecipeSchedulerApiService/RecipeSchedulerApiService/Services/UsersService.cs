using Microsoft.IdentityModel.Tokens;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Types.Inputs;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Services
{
    public class UsersService : IUsersService
    {
        private UserCredentials _tempCreds = new UserCredentials() { Username = "ben", Password = "Passw123" };
        private readonly string _key;

        public UsersService(string key)
        {
            _key = key;
        }

        public async Task<string> Login(UserCredentials userCredentials)
        {
            if (userCredentials.Username != _tempCreds.Username || userCredentials.Password != _tempCreds.Password)
            {
                return null;
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(_key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userCredentials.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature) 
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
