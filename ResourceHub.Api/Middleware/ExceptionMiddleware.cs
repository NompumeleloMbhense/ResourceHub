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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;

            object result;

            switch (exception)
            {
                case ArgumentException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = new { message = ex.Message };
                    break;
                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    result = new { message = "Resource not found" };
                    break;
                case BookingNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    result = new { message = ex.Message };
                    break;
                case BookingConflictException ex:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    result = new { message = ex.Message };
                    break;
                case ResourceNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    result = new { message = ex.Message };
                    break;
                case ResourceUnavailableException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = new { message = ex.Message };
                    break;
                case ResourceHasBookingsException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest; // or 409 if you prefer
                    result = new { message = ex.Message };
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result = new { message = "An unexpected error occurred" };
                    break;
            }

            var json = JsonSerializer.Serialize(result);
            return context.Response.WriteAsync(json);
        }
    }
}