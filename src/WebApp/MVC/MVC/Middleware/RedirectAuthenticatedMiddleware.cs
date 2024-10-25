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
            if (context.User.Identity!.IsAuthenticated && (context.Request.Path == "/" || context.Request.Path == "/home"))
            {
                context.Response.Redirect("/Newsfeed/Index");
                return;
            }

            await _next(context);
        }
    }
}
