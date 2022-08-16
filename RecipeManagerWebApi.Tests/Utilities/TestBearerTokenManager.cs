using Microsoft.Extensions.Configuration;
using RecipeManagerWebApi.Utilities;

namespace RecipeManagerWebApi.Tests.Utilities
{
    public static class TestBearerTokenManager
    {
        public static string getBearerToken()
        {
            //Acts as a quick way of accessing a bearer token for a test without all of the login processes.

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            JwtBearerAuthenticationManager jwtBearerAuthenticationManager = new JwtBearerAuthenticationManager(configuration);

            string bearerToken = jwtBearerAuthenticationManager.GetBearerToken("Ben");

            return bearerToken;
        }
    }
}
