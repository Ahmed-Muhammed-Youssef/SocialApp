using Domain.Constants;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

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
