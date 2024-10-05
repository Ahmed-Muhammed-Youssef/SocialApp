using Domain.Constants;
using Domain.Entities;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure.Identity;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            ILogger<DatabaseInitializer> logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();
            try
            {
                DataContext dataContext = services.GetRequiredService<DataContext>();
                IdentityDatabaseContext identityContext = services.GetRequiredService<IdentityDatabaseContext>();
                IWebHostEnvironment environment = services.GetRequiredService<IWebHostEnvironment>();
                UserManager<IdentityUser> userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                IConfiguration configuration = services.GetRequiredService<IConfiguration>();

                await MigrateDatabaseAsync(dataContext, identityContext, logger);
                await SeedStaticData(dataContext, roleManager, logger);
                await SeedAdmin(userManager, configuration, identityContext);

                if (environment.IsDevelopment())
                {
                    // seed test records
                    await AddTestUsers(userManager, configuration);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database initialization");
            }
        }

        private static async Task MigrateDatabaseAsync(DataContext dataContext, IdentityDatabaseContext identityContext, ILogger<DatabaseInitializer> logger)
        {
            try
            {
                await dataContext.Database.MigrateAsync();
                await identityContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration");
            }
        }

        private static async Task SeedStaticData(DataContext dataContext, RoleManager<IdentityRole> roleManager, ILogger<DatabaseInitializer> logger)
        {
            try
            {
                await dataContext.Database.MigrateAsync();
                await SeedCountriesAndCities(dataContext);

                if (!roleManager.Roles.Any())
                {
                    await SeedRoles(roleManager);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during seeding static data");
            }
        }

        private static async Task SeedCountriesAndCities(DataContext dataContext)
        {
            if (!await dataContext.Cities.AnyAsync())
            {
                string scriptsPath = Directory.GetCurrentDirectory();

                scriptsPath = scriptsPath[..scriptsPath.LastIndexOf("src")];

                scriptsPath = Path.Combine(scriptsPath, "scripts");

                scriptsPath = Path.Combine(scriptsPath, "CitySeedingScripts");

                foreach (var filePath in Directory.GetFiles(scriptsPath))
                {
                    StreamReader streamReader = new(filePath);

                    while (!streamReader.EndOfStream)
                    {
                        StringBuilder script = new();

                        while(script.Length < 200000 && !streamReader.EndOfStream)
                        {
                            script.AppendLine(streamReader.ReadLine());
                        }

                        // this mechanism is vulnerable to sql injection, but the files are on the server (the attacker needs to access the files)
                        // for now it's okay, but needs to be changed in the future
                        await dataContext.Database.ExecuteSqlRawAsync(script.ToString());
                    }
                }
            }
        }

        private static async Task SeedAdmin(UserManager<IdentityUser> userManager, IConfiguration configuration, IdentityDatabaseContext identityContext)
        {
            if(!await userManager.Users.AnyAsync(u => u.Email == configuration["AdminCred:Email"]))
            {
                IdentityUser admin = new()
                {
                    UserName = configuration["AdminCred:Email"],
                    Email = configuration["AdminCred:Email"]
                };

                await userManager.CreateAsync(admin, configuration["AdminCred:Password"]);
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
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
            }
        }

        private static async Task AddTestUsers(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            if (await userManager.Users.AnyAsync(u => u.Email != configuration["AdminCred:Email"])) return;

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
                // await userManager.AddToRolesAsync(user, [RolesNameValues.User]);

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
