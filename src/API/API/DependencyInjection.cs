namespace API;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddGenericServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy => policy.WithOrigins("https://localhost:4200", "http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        });

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
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddScoped<IMessageNotifier, MessageNotifier>();

        builder.Services.AddHttpClient("GoogleAuth", client =>
        {
            client.BaseAddress = new Uri("https://www.googleapis.com/");
            client.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });

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
        builder.Services.AddScoped<PasswordGenerationService>();

        return builder;
    }
}
