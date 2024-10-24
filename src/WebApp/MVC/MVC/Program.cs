using Application.Interfaces.Repositories;
using Application.Interfaces;
using Domain.Constants;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories.CachedRepositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Infrastructure.MappingProfiles;
using Application.Interfaces.Services;
using Infrastructure.ExternalServices.Cloudinary;

namespace MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataContext>();
            builder.Services.AddDbContext<IdentityDatabaseContext>();

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

            builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => policy.RequireRole(RolesNameValues.Admin))
            .AddPolicy("RequireModeratorOrAdmin", policy => policy.RequireRole(RolesNameValues.Admin, RolesNameValues.Moderator));
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages();

            // Repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICachedApplicationUserRepository, CachedUserRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<IPictureRepository, PictureRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IFriendRequestRepository, FriendRequestsRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            builder.Services.AddScoped<IPictureService, PictureService>();

            var app = builder.Build();

            // Prepare Database
            await DatabaseInitializer.InitializeAsync(app.Services);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
