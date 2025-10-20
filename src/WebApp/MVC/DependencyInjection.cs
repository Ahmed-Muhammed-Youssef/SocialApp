using Application.MappingProfiles;
using Domain.Constants;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using MVC.Factories;

namespace MVC;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddGenericServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAntiforgery(options =>
        {
            options.FormFieldName = "AntiforgeryFieldname";
            options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
            options.SuppressXFrameOptionsHeader = false;
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddSignalR();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        return builder;
    }
    public static IHostApplicationBuilder AddIdentity(this IHostApplicationBuilder builder)
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
}
