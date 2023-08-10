using API.Data.Static_Values;
using API.Entities;
using Bogus;
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
            // add admin
            if (await userManager.Users.AnyAsync()) return;
            var roles = new List<AppRole>
            {
                new AppRole{Name = RolesNameValues.Admin},
                new AppRole{Name = RolesNameValues.Moderator},
                new AppRole{Name = RolesNameValues.User}
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
            await userManager.AddToRolesAsync(admin, new[] { RolesNameValues.User, RolesNameValues.Admin, RolesNameValues.Moderator });
            char[] sex = { 'm', 'f' };
            var tasks = new List<Task>();
            for (int i = 0; i < 10000; i++)
            {
                var testUsers = new Faker<AppUser>()
                    .RuleFor(u => u.UserName, (f, u) => $"user{i}")
                    .RuleFor(u => u.Sex, f => f.PickRandom(new List<char>() { 'f', 'm' }))
                    .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName((u.Sex == 'm') ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
                    .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(Bogus.DataSets.Name.Gender.Male))
                    .RuleFor(u => u.Email, (f, u) => $"user{i}@test")
                    .RuleFor(u => u.Bio, f => f.Lorem.Paragraph())
                    .RuleFor(u => u.Interest, f => f.PickRandom(new List<char>() { 'f', 'm', 'b' }))
                    .RuleFor(u => u.City, f => f.Address.City())
                    .RuleFor(u => u.Country, f => f.Address.Country())
                    .RuleFor(u => u.DateOfBirth, f => f.Date.Past(refDate: DateTime.UtcNow.AddYears(-18), yearsToGoBack: 70))
                    .RuleFor(u => u.LastActive, f => f.Date.Recent())
                    .RuleFor(u => u.Created, f => f.Date.Past(refDate: DateTime.UtcNow.AddMonths(-5), yearsToGoBack: 2));

                var user = testUsers.Generate();
                tasks.Add(userManager.CreateAsync(user, "Pwd12345"));
                tasks.Add(userManager.AddToRolesAsync(user, new[] { RolesNameValues.User }));
            }
            await Task.WhenAll(tasks);
        }
    }
}
