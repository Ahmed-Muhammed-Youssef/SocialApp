using Domain.Constants;
using Domain.Entities;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure.Identity;

namespace Infrastructure.Data
{
    public class DatabaseSeeding
    {
        public static async Task MigrateDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<DatabaseSeeding>>();
                logger.LogError(ex, "An error occurred during migration");
            }
        }
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                IdentityDatabaseContext context = services.GetRequiredService<IdentityDatabaseContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await AddData(userManager, roleManager, context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<DatabaseSeeding>>();
                logger.LogError(ex, "An error occurred during seeding");
            }
        }
        private static async Task AddData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IdentityDatabaseContext identityContext)
        {
            // add admin
            if (await userManager.Users.AnyAsync()) return;
            var roles = new List<IdentityRole>
            {
                new() {Name = RolesNameValues.Admin},
                new() {Name = RolesNameValues.Moderator},
                new() {Name = RolesNameValues.User}
            };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            IdentityUser admin = new()
            {
                UserName = "admin@test",
                Email = "admin@test",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            await userManager.CreateAsync(admin, "Pwd12345");
            //await userManager.AddToRolesAsync(admin, [RolesNameValues.Admin]);

            Random random = new();
            for (int i = 0; i < 100; i++)
            {
                var testUsers = new Faker<IdentityUser>()
                    .RuleFor(u => u.UserName, (f, u) => $"user{i}@test")
                    //.RuleFor(u => u.ProfilePictureUrl, f => f.Internet.Avatar())
                    //.RuleFor(u => u.Sex, f => f.PickRandom(new List<char>() { 'f', 'm' }))
                    //.RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName((u.Sex == 'm') ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
                    //.RuleFor(u => u.LastName, (f, u) => f.Name.LastName(Bogus.DataSets.Name.Gender.Male))
                    .RuleFor(u => u.Email, (f, u) => $"user{i}@test")
                    //.RuleFor(u => u.Bio, f => f.Lorem.Paragraph())
                    //.RuleFor(u => u.Interest, f => f.PickRandom(new List<char>() { 'f', 'm', 'b' }))
                    //.RuleFor(u => u.Country, f => f.Address.Country())
                    //.RuleFor(u => u.City, f => f.Address.City())
                    //.RuleFor(u => u.DateOfBirth, f => f.Date.Past(refDate: DateTime.UtcNow.AddYears(-18), yearsToGoBack: 70))
                    //.RuleFor(u => u.LastActive, f => f.Date.BetweenOffset(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(-30)).DateTime)
                    //.RuleFor(u => u.Created, f => f.Date.Past(refDate: DateTime.UtcNow.AddMonths(-7), yearsToGoBack: 2))
                    .RuleFor(u => u.SecurityStamp, (f, u) => Guid.NewGuid().ToString());

                var user = testUsers.Generate();
                await userManager.CreateAsync(user, "Pwd12345");
                await userManager.AddToRolesAsync(user, [RolesNameValues.User]);

                // Add some posts to that user

                Faker<Post> postsFaker = new Faker<Post>()
                    .RuleFor(p => p.Content, f => f.Lorem.Paragraph())
                    .RuleFor(p => p.DatePosted, f => f.Date.Past(3))
                    .RuleFor(p =>p.DateEdited, f => f.Date.Recent(30));
                
                int randomNumber = random.Next(0, 101);
                List<Post> fakePosts = postsFaker.Generate(randomNumber);

                //foreach (var post in fakePosts)
                //{
                //    post.UserId = user.Id;
                //    identityContext.Posts.Add(post);
                //    identityContext.SaveChanges();
                //}
            }
        }
    }
}
