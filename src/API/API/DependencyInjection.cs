using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace API;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddGenericServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        });

        builder.Services.AddCors(options =>
        {
            string[] origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!;
            options.AddPolicy("AllowSpecificOrigin", policy => policy.WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Location")
                .AllowCredentials());
        });

        // Health checks (database liveness)
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDatabaseContext>(
                name: "database",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["ready"])
            .AddCheck(
                "self",
                () => HealthCheckResult.Healthy(),
                tags: ["live"]);

        builder.Services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        builder.Services.AddScoped<LogUserActivity>();
        builder.Services.AddSignalR();
        builder.Services.AddMediator(config =>
        {
            config.ServiceLifetime = ServiceLifetime.Scoped;
        });
        builder.Services.AddScoped<IMessageNotifier, MessageNotifier>();

        builder.Services.AddHttpClient("GoogleAuth", client =>
        {
            client.BaseAddress = new Uri("https://www.googleapis.com/");
            client.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });

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
                .AddRuntimeInstrumentation());

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }
        else
        {
            builder.Services.AddOpenTelemetry().UseAzureMonitor();
        }

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
        });

        // Filter out duplicate SQL queries from logs (captured by tracing)
        builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

        return builder;
    }

    public static IHostApplicationBuilder AddIdentity(this IHostApplicationBuilder builder)
    {
        builder.Services.AddIdentityCore<IdentityUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.SignIn.RequireConfirmedAccount = false;
            opt.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddSignInManager<SignInManager<IdentityUser>>()
        .AddRoleValidator<RoleValidator<IdentityRole>>()
        .AddEntityFrameworkStores<ApplicationDatabaseContext>();

        builder.Services.AddAuthorizationBuilder()
        .AddPolicy("RequireAdminRole", policy => policy.RequireRole(RolesNameValues.Admin))
        .AddPolicy("RequireModeratorOrAdmin", policy => policy.RequireRole(RolesNameValues.Admin, RolesNameValues.Moderator));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
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

        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();

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

        // Add validation filter (FluentValidation validators are registered below)
        builder.Services.AddScoped<ValidationFilter>();
        builder.Services.AddControllers(options =>
        {
            options.Filters.AddService<ValidationFilter>();
        });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        return builder;
    }

    public static IHostApplicationBuilder AddSecurity(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    });
            });

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    await context.HttpContext.Response.WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} seconds.", cancellationToken: token);
                }
                else
                {
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken: token);
                }
            };
        });

        return builder;
    }
}
