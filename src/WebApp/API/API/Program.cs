using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using API.Extensions;
using API.Filters;
using Domain.Configuration;
using Infrastructure.Data;
using Infrastructure.MappingProfiles;
using API.Middleware;
using API.Services;
using API.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Domain;
using Infrastructure.Repositories.CachedRepositories;
using Infrastructure.Repositories;
using Application.Authentication.Google;
using Infrastructure.ExternalServices.Google;
using Infrastructure.ExternalServices.Cloudinary;
using Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddIdentityConfigurations(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICachedApplicationUserRepository, CachedUserRepository>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IPictureRepository, PictureRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestsRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

// User-defined Services
builder.Services.AddSingleton<PresenceTracker>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<PasswordGenerationService>();
builder.Services.AddScoped<LogUserActivity>();

builder.Services.AddDbContext<DataContext>();
builder.Services.AddDbContext<IdentityDatabaseContext>();

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
await DatabaseInitializer.InitializeAsync(app.Services);
// Seeding the application
// await DatabaseSeeding.SeedUsersAsync(app.Services);


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
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "FallBack");

await app.RunAsync();
