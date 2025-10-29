using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Features.Messages;
using Application.Features.Pictures;
using Infrastructure.Data;
using Infrastructure.ExternalServices.Cloudinary;
using Infrastructure.Identity;
using Infrastructure.RealTime.Presence;
using Infrastructure.Repositories;
using Infrastructure.Repositories.CachedRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        // Database Contexts
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));
        builder.Services.AddDbContext<IdentityDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

        // Repositories
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IApplicationUserRepository>(sp =>
        {
            IMemoryCache cache = sp.GetRequiredService<IMemoryCache>();
            DataContext dataContext = sp.GetRequiredService<DataContext>();
            ApplicationUserRepository inner = new(dataContext);
            return new CachedUserRepository(inner, cache, dataContext);
        });
        builder.Services.AddScoped<IPictureRepository, PictureRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<IFriendRequestRepository, FriendRequestsRepository>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository>();
        builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();

        // Cloudinary
        builder.Services.Configure<CloudinaryOptions>(builder.Configuration.GetSection("Cloudinary"));
        builder.Services.AddScoped<IPictureService, PictureService>();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Other Services
        builder.Services.AddSingleton<IOnlinePresenceManager, OnlinePresenceManager>();

        return builder;
    }
}
