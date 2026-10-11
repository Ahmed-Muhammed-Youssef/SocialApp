using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Notifications.API.Common;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        return problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An error occurred while processing your request."
            }
        });
    }
}
