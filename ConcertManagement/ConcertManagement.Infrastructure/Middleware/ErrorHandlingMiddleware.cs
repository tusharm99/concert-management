using System.Text.Json;
using ConcertManagement.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ConcertManagement.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An error occurred while processing your request.",
                    Details = ex.Message
                };
                var jsonResult = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResult);
            }
        }
    }
}
