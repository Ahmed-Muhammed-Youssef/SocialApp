namespace API;

public static class DependecyInjection
{
    public static WebApplicationBuilder AddGenericServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
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

        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        builder.Services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        builder.Services.AddScoped<LogUserActivity>();

        return builder;
    }
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));
        builder.Services.AddDbContext<IdentityDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

        return builder;
    }
    public static WebApplicationBuilder AddRespositories(this WebApplicationBuilder builder)
    {
        // Repositories
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ICachedApplicationUserRepository, CachedUserRepository>();
        builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        builder.Services.AddScoped<IPictureRepository, PictureRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<IFriendRequestRepository, FriendRequestsRepository>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        return builder;
    }
    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityCore<IdentityUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<IdentityRole>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddSignInManager<SignInManager<IdentityUser>>()
        .AddRoleValidator<RoleValidator<IdentityRole>>()
        .AddEntityFrameworkStores<IdentityDatabaseContext>();

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
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        builder.Services.AddScoped<PasswordGenerationService>();

        return builder;
    }
    public static WebApplicationBuilder AddSignalR(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<OnlinePresenceManager>();
        builder.Services.AddSignalR();

        return builder;
    }
    public static WebApplicationBuilder AddPictureStorage(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
        builder.Services.AddScoped<IPictureService, PictureService>();

        return builder;
    }
}
