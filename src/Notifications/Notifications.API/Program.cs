var builder = WebApplication.CreateBuilder(args);

builder
    .AddGenericServices()
    .AddErrorHandling()
    .AddObservability()
    .AddAuth()
    .AddDatabase()
    .AddDelivery();

var app = builder.Build();

await DatabaseInitializer.InitializeAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors(DependencyInjection.CorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapPushEndpoint();
app.MapHub<NotificationsHub>(NotificationsHub.Route);

// Health endpoints
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthCheckResponseFormatter.WriteHealthCheckResponse
});

await app.RunAsync();
