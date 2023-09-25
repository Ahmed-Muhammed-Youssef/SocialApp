using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace API.Helpers
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
            var userId = resultContext.HttpContext.User.GetId();
            var repo = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user = await repo.UserRepository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            repo.UserRepository.Update(user);
            await repo.Complete();
        }
    }
}
