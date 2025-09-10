using Infrastructure.Data;
using MVC;
using MVC.Hubs;
using MVC.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddSignalR()
    .AddDatabase()
    .AddRepositories()
    .AddGenericServices()
    .AddIdentity()
    .AddPictureStorage();

var app = builder.Build();

// Prepare Database
await DatabaseInitializer.InitializeAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<RedirectAuthenticatedMiddleware>();

app.MapRazorPages();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
