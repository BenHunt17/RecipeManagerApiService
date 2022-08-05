using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using RecipeManagerWebApi.Types.Common;
using System;

namespace RecipeManagerWebApi.Utilities
{
    public static class Middleware
    {
        public static IApplicationBuilder useWebApiExceptionHandler(this IApplicationBuilder app)
        {
            //Basically a wrapper for asp.net core's native exception handler with some custom logic which constructs an appropriate response depending on the exception type

            app.UseExceptionHandler(
                options =>
                {
                    options.Run(async context =>
                    {
                        context.Response.ContentType = "text/plain";
                        IExceptionHandlerFeature exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

                        if (exceptionObject == null)
                        {
                            //This should never happen
                            context.Response.StatusCode = 500;
                            await context.Response.WriteAsync("Unknown Exception Occured");

                            return;
                        }

                        Exception exception = exceptionObject.Error;

                        if (exception is WebApiException)
                        {
                            //Every exception in this project should using this object.

                            WebApiException webApiException = exception as WebApiException;

                            context.Response.StatusCode = (int)webApiException.HttpStatusCode; //I know the enum values correspond to the correct code so I do a simple int cast. Naughty
                            await context.Response.WriteAsync(webApiException.Message);

                            return;
                        }

                        //Will just return a 500 Error with whatever message the exception has. This is default behaviour which is to be avoided
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync(exception.Message);
                    });
                }
            );

            return app;
        }
    }
}
