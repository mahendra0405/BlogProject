using BlogLab.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace BlogLab.Web.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType= "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ApiExceptions()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error",
                         
                        }.ToString());
                    }
                });
            }); 
        }
    }
}
