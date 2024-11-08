using API.Errors;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionsMiddleware(RequestDelegate next,
    ILogger<ExceptionsMiddleware> logger, IHostEnvironment env
         )
    {
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await HandleCustomExceptionResponseAsync(context, ex, env);
            }
        }

        private static Task HandleCustomExceptionResponseAsync(HttpContext context, Exception ex,
            IHostEnvironment env)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment() ?
                new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) :
                new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);
            return context.Response.WriteAsync(json);
        }

    }
}
