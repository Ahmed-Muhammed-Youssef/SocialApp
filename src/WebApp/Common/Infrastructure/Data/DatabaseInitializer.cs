﻿using Domain.Constants;
using Domain.Entities;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure.Identity;
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
                await SeedAdmin(userManager, configuration, dataContext, logger);

                if (environment.IsDevelopment())
                {
                    // seed test records
                    await AddTestUsers(userManager, dataContext, configuration, logger);
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
            await SeedCountriesAndCities(dataContext, logger);
            await SeedRoles(roleManager, logger);
        }

        private static async Task SeedCountriesAndCities(DataContext dataContext, ILogger<DatabaseInitializer> logger)
        {
            try
            {
                if (!await dataContext.Countries.AnyAsync(c => c.Code == "EG"))
                {
                    var egypt = new Country
                    {
                        Name = "Egypt",
                        Code = "EG",
                        Language = "ar",
                        Regions =
                        [
                            new Region
                            {
                                Name = "Cairo",
                                Cities =
                                [
                                    new City { Name = "Nasr City" },
                                    new City { Name = "Heliopolis" },
                                    new City { Name = "Maadi" },
                                    new City { Name = "Downtown Cairo" }
                                ]
                            },
                            new Region
                            {
                                Name = "Alexandria",
                                Cities =
                                [
                                    new City { Name = "Montaza" },
                                    new City { Name = "Sidi Gaber" },
                                    new City { Name = "Smouha" },
                                    new City { Name = "Mansheya" }
                                ]
                            },
                            new Region
                            {
                                Name = "Giza",
                                Cities =
                                [
                                    new City { Name = "Dokki" },
                                    new City { Name = "Mohandessin" },
                                    new City { Name = "Haram" },
                                    new City { Name = "Sheikh Zayed" }
                                ]
                            }
                        ]
                    };

                    await dataContext.Countries.AddAsync(egypt);
                    await dataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding countries, regions and cities.");
            }
        }

        private static async Task SeedAdmin(UserManager<IdentityUser> userManager, IConfiguration configuration, DataContext dataContext, ILogger<DatabaseInitializer> logger)
        {
            try
            {
                if (!await userManager.Users.AnyAsync(u => u.Email == configuration["AdminCred:Email"]))
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

                    City city = await dataContext.Cities.FirstOrDefaultAsync() ?? throw new Exception("No cities found in the database. Please ensure that countries and cities are seeded before seeding the admin user.");

                    ApplicationUser adminAppUser = new()
                    {
                        IdentityId = admin.Id,
                        FirstName = "Admin",
                        LastName = "",
                        ProfilePictureUrl = "",
                        Sex = 'm',
                        Bio = "hello there",
                        CityId = city.Id
                    };

                    await dataContext.ApplicationUsers.AddAsync(adminAppUser);
                    await dataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the admin user.");
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager, ILogger<DatabaseInitializer> logger)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding roles.");
            }
        }

        private static async Task AddTestUsers(UserManager<IdentityUser> userManager, DataContext dataContext, IConfiguration configuration, ILogger<DatabaseInitializer> logger)
        {
            try
            {
                if (await userManager.Users.AnyAsync(u => u.Email != configuration["AdminCred:Email"])) return;

                Random random = new();

                var cities = await dataContext.Cities.ToListAsync();
                
                if (cities.Count == 0)
                {
                    throw new Exception("No cities found in the database. Please ensure that countries and cities are seeded before adding test users.");
                }

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
                                .RuleFor(u => u.CityId, f => f.PickRandom(cities).Id)
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
                        .RuleFor(p => p.DateEdited, f => f.Date.Recent(30));

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
                var firstUser = applicationUsers.Where(u => u.IdentityId == firstIdentityUser.Id).FirstOrDefault();

                if (firstUser is not null)
                {
                    foreach (var user in applicationUsers)
                    {
                        if (user.IdentityId != firstIdentityUser.Id)
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
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding test users.");
            }
        }
    }
}
