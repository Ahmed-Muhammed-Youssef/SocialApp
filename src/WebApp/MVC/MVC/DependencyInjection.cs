using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Configuration;
using Domain.Constants;
using Infrastructure.Data;
using Infrastructure.ExternalServices.Cloudinary;
using Infrastructure.Identity;
using Infrastructure.MappingProfiles;
using Infrastructure.RealTime.Presence;
using Infrastructure.Repositories;
using Infrastructure.Repositories.CachedRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.Factories;

namespace MVC;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddGenericServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAntiforgery(options =>
        {
            options.FormFieldName = "AntiforgeryFieldname";
            options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
            options.SuppressXFrameOptionsHeader = false;
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        return builder;
    }
    public static WebApplicationBuilder AddSignalR(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();

        builder.Services.AddSingleton<OnlinePresenceManager>();
        return builder;
    }
    public static WebApplicationBuilder AddRepositories(this WebApplicationBuilder builder)
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
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));
        builder.Services.AddDbContext<IdentityDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

        return builder;
    }
    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddDefaultIdentity<IdentityUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<IdentityRole>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddSignInManager<SignInManager<IdentityUser>>()
        .AddRoleValidator<RoleValidator<IdentityRole>>()
        .AddEntityFrameworkStores<IdentityDatabaseContext>();

        builder.Services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, ApplicationUserClaimsPrincipalFactory<IdentityUser, IdentityRole>>();

        builder.Services.AddAuthorizationBuilder()
        .AddPolicy("RequireAdminRole", policy => policy.RequireRole(RolesNameValues.Admin))
        .AddPolicy("RequireModeratorOrAdmin", policy => policy.RequireRole(RolesNameValues.Admin, RolesNameValues.Moderator));

        return builder;
    }
    public static WebApplicationBuilder AddPictureStorage(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPictureService, PictureService>();
        builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
        return builder;
    }
}
