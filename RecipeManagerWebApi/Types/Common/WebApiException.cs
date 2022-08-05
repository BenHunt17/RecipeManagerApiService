using System;
using System.Net;

namespace RecipeManagerWebApi.Types.Common
{
    public class WebApiException : Exception
    {
        //A custom exception class which capture both HTTP status code and error message.

        public HttpStatusCode HttpStatusCode { get; private set; } //Inherits from Exception sso message is already handled. Tho need to explicitely state status code

        public WebApiException(HttpStatusCode httpStatusCode, string message = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
