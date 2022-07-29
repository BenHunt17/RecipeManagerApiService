using RecipeSchedulerApiService.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RecipeSchedulerApiService.Utilities
{
    public class JwtBearerAuthenticationManager : IJwtBearerAuthenticationManager
    {
        private readonly string _key;

        public JwtBearerAuthenticationManager(string key)
        {
            //Takes a key which is a secure random set of characters
            _key = key;
        }

        public string GetBearerToken(string username)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(); //Initialises a token handler insatcne for creation/validation of tokens

            byte[] tokenKey = Encoding.ASCII.GetBytes(_key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor() //A class for holding attributes about a token
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //Claims identity is a collection of "claims" whichdescribe the entity in question. This entity is the "subject" which is basically the autheniticated user
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature) //Basically the algorithm for generating a token. Uses the secret key as a symmetric key
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor); //Creates a token using the attributes defined above.

            return tokenHandler.WriteToken(token);
        }
    }
}
