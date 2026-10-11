using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Notifications.API;

public static class DependencyInjection
{
    public const string ConnectionStringName = "NotificationsConnection";
    public const string CorsPolicyName = "AllowSpecificOrigin";

    public static IHostApplicationBuilder AddGenericServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.TryAddSingleton(TimeProvider.System);

        builder.Services.AddCors(options =>
        {
            string[] origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
            options.AddPolicy(CorsPolicyName, policy => policy.WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<NotificationsDbContext>(
                name: "database",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["ready"])
            .AddCheck(
                "self",
                () => HealthCheckResult.Healthy(),
                tags: ["live"]);

        return builder;
    }

    public static IHostApplicationBuilder AddErrorHandling(this IHostApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        return builder;
    }

    public static IHostApplicationBuilder AddObservability(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
            .WithTracing(tracing => tracing
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation())
            .WithMetrics(metrics => metrics
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddMeter(DeliveryMetrics.MeterName));

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
        });

        // The delivery workers poll the database; their SQL is captured by tracing, not logs.
        builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

        return builder;
    }

    /// <summary>
    /// Validates the same JWTs the main API issues (same Jwt:* settings), so <c>Context.UserIdentifier</c> on the hub
    /// is the user id from the token's <c>nameid</c> claim.
    /// </summary>
    public static IHostApplicationBuilder AddAuth(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = GetRequiredSetting(builder.Configuration, "Jwt:Issuer"),
                    ValidAudience = GetRequiredSetting(builder.Configuration, "Jwt:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetRequiredSetting(builder.Configuration, "Jwt:Key")))
                };
                options.Events = new JwtBearerEvents
                {
                    // Browsers can't set headers on WebSocket requests, so SignalR sends the token in the query string.
                    OnMessageReceived = context =>
                    {
                        string? accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddOptions<IntakeOptions>()
            .BindConfiguration("Intake")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ApiKey), "Intake:ApiKey must be configured.")
            .ValidateOnStart();

        return builder;
    }

    public static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is not configured.");

        builder.Services.AddDbContext<NotificationsDbContext>(options => ConfigureDbContext(options, connectionString));

        return builder;
    }

    public static IHostApplicationBuilder AddDelivery(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<DeliverySignals>();
        builder.Services.AddSingleton<DeliveryMetrics>();
        builder.Services.AddScoped<DeliveryStore>();
        builder.Services.AddScoped<PushHandler>();

        builder.Services.AddOptions<CleanupOptions>().BindConfiguration("Delivery:Cleanup");
        builder.Services.AddHostedService<DeliveryCleanupWorker>();

        // Channels: each gets its own deliveries, options (Delivery:Channels:{name}) and worker.
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<PresenceTracker>();
        builder.Services.AddNotificationChannel<SignalRChannel>(SignalRChannel.Name);

        return builder;
    }

    public static void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
    }

    private static string GetRequiredSetting(IConfiguration configuration, string key)
        => configuration[key] is { Length: > 0 } value ? value : throw new InvalidOperationException($"Configuration value '{key}' is not set.");
}
