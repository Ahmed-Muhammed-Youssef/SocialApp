var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddIdentityConfigurations(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
});

// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICachedApplicationUserRepository, CachedUserRepository>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IPictureRepository, PictureRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestsRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

// User-defined Services
builder.Services.AddSingleton<OnlinePresenceManager>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<PasswordGenerationService>();
builder.Services.AddScoped<LogUserActivity>();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));
builder.Services.AddDbContext<IdentityDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

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

await app.RunAsync();
