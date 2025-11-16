namespace Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        // Database Contexts
        builder.Services.AddDbContext<ApplicationDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));
        builder.Services.AddDbContext<IdentityDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

        // Repositories
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IApplicationUserRepository>(sp =>
        {
            IMemoryCache cache = sp.GetRequiredService<IMemoryCache>();
            ApplicationDatabaseContext dataContext = sp.GetRequiredService<ApplicationDatabaseContext>();
            ApplicationUserRepository inner = new(dataContext);
            return new CachedUserRepository(inner, cache, dataContext);
        });
        builder.Services.AddScoped<IPictureRepository, PictureRepository>();
        builder.Services.AddScoped<IDirectChatRepository, DirectChatRepository>();
        builder.Services.AddScoped<IFriendRequestRepository, FriendRequestsRepository>();
        builder.Services.AddScoped<IFriendRepository, FriendRepository>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();

        // Cloudinary
        builder.Services.Configure<CloudinaryOptions>(builder.Configuration.GetSection("Cloudinary"));
        builder.Services.AddScoped<IPictureService, CloudinaryPictureService>();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Other Services
        builder.Services.AddSingleton<IOnlineUsersStore, OnlineUsersStore>();
        builder.Services.AddSingleton<IDirectChatGroupsStore, DirectChatGroupsStore>();

        builder.Services.Configure<JwtAuthOptions>(builder.Configuration.GetSection("Jwt"));

        return builder;
    }
}
