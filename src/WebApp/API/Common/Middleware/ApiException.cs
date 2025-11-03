namespace API.Common.Middleware;

public class ApiException(int statusCode, string? message = null, string? details = null)
{
    public int StatusCode { get; } = statusCode;
    public string? Message { get; } = message;
    public string? Details { get; } = details;

}
