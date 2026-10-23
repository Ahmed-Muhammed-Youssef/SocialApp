using Notifications.API;
using Notifications.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.AddDatabase();
builder.AddValidation();
builder.AddJwtAuthentication();
builder.AddSignalRServices();
builder.AddCorsPolicy();

var app = builder.Build();

await DatabaseInitializer.InitializeAsync(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(DependencyInjection.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapNotificationEndpoints();

await app.RunAsync();

// Marker so WebApplicationFactory<Program> can reference the entry-point assembly.
public partial class Program;
