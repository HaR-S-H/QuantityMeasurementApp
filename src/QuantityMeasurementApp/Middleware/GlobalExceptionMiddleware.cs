using System.Net;
using System.Text.Json;
using QuantityMeasurementApp.Business.Exceptions;

namespace QuantityMeasurementApp.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (QuantityMeasurementException ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                await WriteErrorAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "An unexpected error occurred."
                );
            }
        }

        private static async Task WriteErrorAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message
        )
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var payload = JsonSerializer.Serialize(new { error = message });
            await context.Response.WriteAsync(payload);
        }
    }
}
