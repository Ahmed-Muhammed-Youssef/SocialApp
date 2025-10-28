using Application.Common.Interfaces;

namespace API.Filters;

public class LogUserActivity(IUnitOfWork unitOfWork) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        if (resultContext.HttpContext.User.Identity is not { IsAuthenticated: true })
            return;

        var userId = resultContext.HttpContext.User.GetPublicId();
        var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
        if (user is null) return;

        user.LastActive = DateTime.UtcNow;
        unitOfWork.ApplicationUserRepository.Update(user);
        await unitOfWork.SaveChangesAsync();
    }
}
