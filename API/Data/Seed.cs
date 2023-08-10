using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                    await AddData(userManager, roleManager);
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }

           
        }
        private static async Task AddData(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync())
                return;
            var roles = new List<AppRole>
            {
                new AppRole{Name = "user"},
                new AppRole{Name = "admin"},
                new AppRole{Name = "moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
            var admin = new AppUser
            {
                UserName = "admin",
                FirstName = "admin",
                LastName = "admin",
                Interest = 'f',
                DateOfBirth = DateTime.UtcNow,
                LastActive = DateTime.UtcNow,
                Email = "admin@test",
                City = "admin",
                Country = "admin",
                Created = DateTime.UtcNow,
                Sex = 'm'
            };
            await userManager.CreateAsync(admin, "Pwd12345");
            await userManager.AddToRolesAsync(admin, new[] { "user", "admin", "moderator" });
        }
    }
}
