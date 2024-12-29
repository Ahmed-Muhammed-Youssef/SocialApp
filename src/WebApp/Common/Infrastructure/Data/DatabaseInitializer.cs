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
                await SeedAdmin(userManager, configuration, identityContext, dataContext);

                if (environment.IsDevelopment())
                {
                    // seed test records
                    await AddTestUsers(userManager, dataContext, configuration);
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

        private static async Task SeedAdmin(UserManager<IdentityUser> userManager, IConfiguration configuration, IdentityDatabaseContext identityContext, DataContext dataContext)
        {
            if(!await userManager.Users.AnyAsync(u => u.Email == configuration["AdminCred:Email"]))
            {
                IdentityUser admin = new()
                {
                    UserName = configuration["AdminCred:Email"],
                    Email = configuration["AdminCred:Email"]
                };

                await userManager.CreateAsync(admin, configuration["AdminCred:Password"]);

                await userManager.AddToRoleAsync(admin, RolesNameValues.Admin);
                await userManager.AddToRoleAsync(admin, RolesNameValues.User);
                await userManager.AddToRoleAsync(admin, RolesNameValues.Moderator);

                ApplicationUser adminAppUser = new() { 
                    IdentityId = admin.Id,
                    FirstName = "Admin",
                    LastName = "",
                    ProfilePictureUrl = "",
                    Sex = 'm',
                    Bio = "hello there",
                    CityId = 2000,
                };

                await dataContext.ApplicationUsers.AddAsync(adminAppUser);
                await dataContext.SaveChangesAsync();
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

        private static async Task AddTestUsers(UserManager<IdentityUser> userManager, DataContext dataContext, IConfiguration configuration)
        {
            if (await userManager.Users.AnyAsync(u => u.Email != configuration["AdminCred:Email"])) return;

            Random random = new();
            for (int i = 0; i < 100; i++)
            {
                // Generate Idneity User
                var testIdentityUsers = new Faker<IdentityUser>()
                    .RuleFor(u => u.UserName, (f, u) => $"user{i}@test")
                    .RuleFor(u => u.Email, (f, u) => $"user{i}@test")
                    .RuleFor(u => u.SecurityStamp, (f, u) => Guid.NewGuid().ToString());

                var identityUser = testIdentityUsers.Generate();
                await userManager.CreateAsync(identityUser, "Pwd12345");
                await userManager.AddToRoleAsync(identityUser, RolesNameValues.User);

                // Generate application user
                var testApplicationUser = new Faker<ApplicationUser>()
                            .RuleFor(u => u.ProfilePictureUrl, f => f.Internet.Avatar())
                            .RuleFor(u => u.Sex, f => f.PickRandom(new List<char>() { 'f', 'm' }))
                            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName((u.Sex == 'm') ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
                            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(Bogus.DataSets.Name.Gender.Male))
                            .RuleFor(u => u.Bio, f => f.Lorem.Paragraph())
                            .RuleFor(u => u.CityId, f => f.Random.Number(1, 100000))
                            .RuleFor(u => u.DateOfBirth, f => f.Date.Past(refDate: DateTime.UtcNow.AddYears(-18), yearsToGoBack: 70))
                            .RuleFor(u => u.LastActive, f => f.Date.BetweenOffset(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(-30)).DateTime)
                            .RuleFor(u => u.Created, f => f.Date.Past(refDate: DateTime.UtcNow.AddMonths(-7), yearsToGoBack: 2));

                var applicationUser = testApplicationUser.Generate();
                applicationUser.IdentityId = identityUser.Id;

                await dataContext.ApplicationUsers.AddAsync(applicationUser);

                await dataContext.SaveChangesAsync();

                // Add some posts to that user
                Faker<Post> postsFaker = new Faker<Post>()
                    .RuleFor(p => p.Content, f => f.Lorem.Paragraph())
                    .RuleFor(p => p.DatePosted, f => f.Date.Past(3))
                    .RuleFor(p =>p.DateEdited, f => f.Date.Recent(30));
                
                int randomNumber = random.Next(0, 101);
                List<Post> fakePosts = postsFaker.Generate(randomNumber);

                foreach (var post in fakePosts)
                {
                    post.UserId = applicationUser.Id;
                    dataContext.Posts.Add(post);
                    dataContext.SaveChanges();
                }

            }

            // add all users as friends to user1
            if (dataContext.Friends.Any()) return;
            var firstIdentityUser = await userManager.Users.Where(u => u.Email == "user1@test").FirstOrDefaultAsync();
            var applicationUsers = await dataContext.ApplicationUsers.ToListAsync();
            var firstUser = applicationUsers.Where(u => u.IdentityId ==  firstIdentityUser.Id).FirstOrDefault();    

            if(firstUser is not null)
            {
                foreach (var user in applicationUsers)
                {
                    if(user.IdentityId != firstIdentityUser.Id)
                    {

                        await dataContext.Friends.AddAsync(new Friend()
                        {
                            UserId = firstUser.Id,
                            FriendId = user.Id
                        });

                        await dataContext.Friends.AddAsync(new Friend()
                        {
                            UserId = user.Id,
                            FriendId = firstUser.Id
                        });
                    }
                }

                await dataContext.SaveChangesAsync();
            }
        }
    }
}
