using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace RecipeManagerWebApi.Tests.Utilities
{
    public static class HttpRequestBuilder
    {
        //Series of static methods effectively abstracting away boiler plate HTTP request building

        public static HttpRequestMessage BuildRequest(string uri, HttpMethod httpMethod)
        {
            //A wrapper for creating a request. Doesn't abstract much but it makes chaining these methods look nice in tests.
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, uri);

            return request;
        }

        public static HttpRequestMessage AddJsonBody<T>(this HttpRequestMessage httpRequestMessage, T content)
        {
            //Serialises an object into JSON and assigns it to the request
            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            return httpRequestMessage;
        }

        public static HttpRequestMessage AddFormBody<T>(this HttpRequestMessage httpRequestMessage, T content)
        {
            //Add an object to an HTTP request in the form of formData.

            if(content == null)
            {
                return httpRequestMessage;
            }

            MultipartFormDataContent formData = new MultipartFormDataContent();

            if (content is FileStream fileStream)
            {
                formData.Add(new StreamContent(fileStream), "ImageFile", "file");
            }
            else
            {
                foreach (PropertyInfo property in content.GetType().GetProperties())
                {
                    //Loops through each property in the object and depending on what its type is, will cast it to a string in some way then append it to the form data
                    string value;
                    object? rawValue = property.GetValue(content);

                    if (rawValue == null)
                    {
                        continue;
                    }

                    if (rawValue is FileStream fileValue)
                    {
                        formData.Add(new StreamContent(fileValue), "ImageFile", "file");
                    }

                    if (rawValue.GetType().IsPrimitive || rawValue.GetType() == typeof(decimal) || rawValue.GetType() == typeof(string))
                    {
                        value = rawValue?.ToString() ?? "";
                    }
                    else
                    {
                        value = JsonConvert.SerializeObject(rawValue);
                    }

                    formData.Add(new StringContent(value), property.Name);
                }
            }

            httpRequestMessage.Content = formData;

            return httpRequestMessage;
        }
    }
}
