namespace API.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
            var userId = resultContext.HttpContext.User.GetPublicId().Value;
            var repo = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user = await repo.ApplicationUserRepository.GetByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            repo.ApplicationUserRepository.Update(user);
            await repo.SaveChangesAsync();
        }
    }
}
