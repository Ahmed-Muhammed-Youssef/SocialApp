using Application.Common.Interfaces;

namespace API.Filters;

public class LogUserActivity(IUnitOfWork unitOfWork) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cancellationToken = context.HttpContext.RequestAborted;

        var resultContext = await next();
        if (resultContext.HttpContext.User.Identity is not { IsAuthenticated: true })
            return;

        var userId = resultContext.HttpContext.User.GetPublicId();
        var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null) return;

        user.LastActive = DateTime.UtcNow;
        await unitOfWork.ApplicationUserRepository.UpdateAsync(user, cancellationToken);
    }
}
