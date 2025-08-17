using System.Net;
using Newtonsoft.Json;

namespace SystemPortal.Api.ExceptionHandling
{
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlingMiddleware> _logger;

        public CustomExceptionHandlingMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                HttpStatusCode code;

                switch (exception)
                {
                    case KeyNotFoundException
                        or FileNotFoundException:
                        code = HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedAccessException:
                        code = HttpStatusCode.Unauthorized;
                        break;
                    case ArgumentException
                        or InvalidOperationException:
                        code = HttpStatusCode.BadRequest;
                        break;
                    default:
                        code = HttpStatusCode.InternalServerError;
                        break;
                }

                _logger.LogError($"Error Message: {exception.Message}, Time of occurrence {DateTime.Now}");

                context.Response.StatusCode = (int) code;
                context.Response.ContentType = "application/json";

                // Optionally, return a custom error response
                await context.Response.WriteAsync(JsonConvert.SerializeObject("An unexpected error occurred."));
            }
        }
    }
}
