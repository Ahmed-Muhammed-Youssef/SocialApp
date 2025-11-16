namespace API.Common.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env, JsonSerializerOptions jsonOptions)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError("Error Message: {exceptionMessage}, Time of occurence {time}", ex.Message, DateTime.UtcNow);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ApiException response = env.IsDevelopment() ? new ApiException(httpContext.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiException(httpContext.Response.StatusCode, "Internal Server Error");

            string json = JsonSerializer.Serialize(response, jsonOptions);

            await httpContext.Response.WriteAsync(json);
        }
    }
}
