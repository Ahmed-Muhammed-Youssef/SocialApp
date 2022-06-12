using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
           RoleManager<AppRole> roleManager)
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
                DateOfBirth = System.DateTime.UtcNow,
                LastActive = System.DateTime.UtcNow,
                Email = "admin",
                City = "admin",
                Country = "admin",
                Created = System.DateTime.UtcNow,
                Sex = 'm'
            };
            await userManager.CreateAsync(admin, "Pwd12345");
            await userManager.AddToRolesAsync(admin, new[] {"user", "admin", "moderator" });
        }
    }
}
