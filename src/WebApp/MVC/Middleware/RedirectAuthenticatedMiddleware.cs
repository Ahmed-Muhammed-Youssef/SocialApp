namespace MVC.Middleware
{
    public class RedirectAuthenticatedMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectAuthenticatedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if((context.Request.Path == "/" || context.Request.Path == "/home"))
            {
                if (context.User.Identity!.IsAuthenticated)
                {
                    context.Response.Redirect("/Newsfeed/Index");
                    return;
                }
                else
                {
                    context.Response.Redirect("/Identity/Account/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}
