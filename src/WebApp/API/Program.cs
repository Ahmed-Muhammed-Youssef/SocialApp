var builder = WebApplication.CreateBuilder(args);

builder
    .AddGenericServices()
    .AddIdentity()
    .AddInfrastructureServices();

var app = builder.Build();

// Initialize Database
await DatabaseInitializer.InitializeAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowSpecificOrigin");
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

// SignalR Endpooints
app.MapHub<OnlineUsersHub>("hubs/presence");
app.MapHub<ChatHub>("hubs/message");

await app.RunAsync();
