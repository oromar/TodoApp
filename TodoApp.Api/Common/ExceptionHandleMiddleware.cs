using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace TodoApp.Api.Common
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                const string err = "errors";

                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                httpContext.Response.Clear();
                httpContext.Response.ContentType = "application/json";

                var statusCode = (int)HttpStatusCode.BadRequest;
                var message = new Dictionary<string, string[]>();
                var messageText = ex.InnerException?.Message ?? ex.Message;
                message.Add(err, new string[] { messageText });
                httpContext.Response.StatusCode = statusCode;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(message));
            }
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
