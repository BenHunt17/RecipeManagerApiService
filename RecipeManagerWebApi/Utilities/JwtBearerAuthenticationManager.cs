using RecipeManagerWebApi.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace RecipeManagerWebApi.Utilities
{
    public class JwtBearerAuthenticationManager : IJwtBearerAuthenticationManager
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtBearerAuthenticationManager(IConfiguration configuration)
        {
            _key = configuration.GetValue<string>("JwtBearer:key");
            _issuer = configuration.GetValue<string>("JwtBearer:issuer");
            _audience = configuration.GetValue<string>("JwtBearer:audience");
        }

        public string GetBearerToken(string username)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(); //Initialises a token handler insatcne for creation/validation of tokens

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor() //A class for holding attributes about a token
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //Claims identity is a collection of "claims" whichdescribe the entity in question. This entity is the "subject" which is basically the autheniticated user
                    new Claim("username", username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(CreateSecurityKey(), SecurityAlgorithms.HmacSha256Signature) //Basically the algorithm for generating a token. Uses the secret key as a symmetric key
        };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor); //Creates a token using the attributes defined above.

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateUser(string bearerToken, string username)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(bearerToken,
                    new TokenValidationParameters()
                    {
                        ValidIssuer = _issuer,
                        ValidAudience = _audience,
                        IssuerSigningKey = CreateSecurityKey()
                    },
                    out SecurityToken securityToken);
            }
            catch
            {
                return false;
            }

            JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(bearerToken);

            Claim usernameClaim = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "username");
            if(usernameClaim == null)
            {
                return false;
            }

            string usernameClaimValue = usernameClaim.Value;
            if (usernameClaimValue == null)
            {
                return false;
            }

            return usernameClaimValue == username;
        }

        private SymmetricSecurityKey CreateSecurityKey()
        {
            byte[] tokenKey = Encoding.ASCII.GetBytes(_key);
            return new SymmetricSecurityKey(tokenKey);
        }
    }
}
