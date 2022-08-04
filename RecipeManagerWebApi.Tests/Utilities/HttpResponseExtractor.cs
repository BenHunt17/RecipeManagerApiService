
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Tests.Utilities
{
    public static class HttpResponseExtractor
    {
        public async static Task<string> GetStringResult(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();

            return content;
        }

        public async static Task<T> GetObjectResult<T>(HttpResponseMessage response)
        {
            T content = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

            return content;
        }
    }
}
