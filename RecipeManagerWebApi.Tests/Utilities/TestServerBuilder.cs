using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using RecipeManagerWebApi;

namespace RecipeManagerWebApi.Tests.Utilities
{
    public static class TestServerBuilder
    {
        public static TestServer buildServer()
        {
            //Abstracts away the test server boilerplate. It's not loads of code but it clutters up the test suite constructors

            WebHostBuilder webHostBuilder = new WebHostBuilder();
            webHostBuilder
                .UseConfiguration(new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddJsonFile("appsettings.json").Build())
                .UseStartup<Startup>();

            return new TestServer(webHostBuilder);
        }
    }
}
