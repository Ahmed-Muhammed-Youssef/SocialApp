using API.Application.Interfaces;
using API.Application.Interfaces.Repositories;
using API.Application.Interfaces.Services;
using API.Extensions;
using API.Filters;
using API.Domain.Configuration;
using API.Infrastructure.Data;
using API.Infrastructure.MappingProfiles;
using API.Middleware;
using API.Services;
using API.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using API.Infrastructure.Services;
using API.Infrastructure.Repositories.CachedRepositories;
using API.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddIdentityConfigurations(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICachedUserRepository, CachedUserRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPictureRepository, PictureRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestsRepository>();

// User-defined Services
builder.Services.AddSingleton<PresenceTracker>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<LogUserActivity>();
builder.Services.AddDbContext<DataContext>(options =>
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    // Depending on if in development or production, use either Heroku-provided
    // connection string, or development connection string from env var.
    if (env == "Development")
    {
        // Use connection string from file.
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    else
    {
        // production configurations
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionConnection"));
    }
});
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy => policy.AllowAnyMethod()
    .AllowAnyHeader().WithOrigins("https://localhost:4200").AllowCredentials());
});


var app = builder.Build();

// Migrate Database
await DatabaseSeeding.MigrateDatabaseAsync(app.Services);
// Seeding the application
await DatabaseSeeding.SeedUsersAsync(app.Services);


if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowSpecificOrigin");
}

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();


app.MapControllers();

// SignalR Endpooints
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "FallBack");

await app.RunAsync();
