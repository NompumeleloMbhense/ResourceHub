using System.Net;
using System.Text.Json;
using ResourceHub.Core.Exceptions;

/// <summary>
/// Middleware for global exception handling
/// Catches unhandled exceptions and returns appropriate HTTP responses
/// </summary>

namespace ResourceHub.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        // Handles exceptions and generates appropriate HTTP responses based on the type of exception
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is AppException appEx)
            {
                context.Response.StatusCode = appEx.StatusCode;

                return context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    message = appEx.Message
                }));
            }

            // For unhandled exceptions, return a generic 500 Internal Server Error response
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "An unexpected error occurred"
            }));
        }
    }
}