namespace API.Errors;

public class ApiException(int statusCode, string message = null, string details = null)
{
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public string Details { get; set; } = details;

}
