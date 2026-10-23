using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Notifications.API.Hubs;

namespace Notifications.API;

public static class DependencyInjection
{
    public const string ConnectionStringName = "NotificationsConnection";
    public const string CorsPolicyName = "AllowSpecificOrigin";

    public static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is not configured.");

        builder.Services.AddDbContext<NotificationsDbContext>(options => ConfigureDbContext(options, connectionString));
        return builder;
    }

    /// <summary>Shared by the app and <see cref="DesignTimeDbContextFactory"/> so migrations see the same model.</summary>
    public static void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
    }

    public static IHostApplicationBuilder AddJwtAuthentication(this IHostApplicationBuilder builder)
    {
        string key = builder.Configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        string issuer = builder.Configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
        string audience = builder.Configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };

                // Allow SignalR web sockets to read access_token from query string
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();
        return builder;
    }

    public static IHostApplicationBuilder AddValidation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        return builder;
    }

    public static IHostApplicationBuilder AddSignalRServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<IUserIdProvider, NameIdUserIdProvider>();
        return builder;
    }

    public static IHostApplicationBuilder AddCorsPolicy(this IHostApplicationBuilder builder)
    {
        string[] origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? ["http://localhost:4200", "https://localhost:4200"];

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
                policy.WithOrigins(origins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
        });

        return builder;
    }
}
