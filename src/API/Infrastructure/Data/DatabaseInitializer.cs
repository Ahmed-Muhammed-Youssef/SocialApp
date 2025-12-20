namespace Infrastructure.Data;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        ILogger<DatabaseInitializer> logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();
        try
        {
            ApplicationDatabaseContext dataContext = services.GetRequiredService<ApplicationDatabaseContext>();
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

    private static async Task MigrateDatabaseAsync(ApplicationDatabaseContext dataContext, IdentityDatabaseContext identityContext, ILogger<DatabaseInitializer> logger)
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

    private static async Task SeedStaticData(ApplicationDatabaseContext dataContext, RoleManager<IdentityRole> roleManager, ILogger<DatabaseInitializer> logger)
    {
        await SeedCountriesAndCities(dataContext, logger);
        await SeedRoles(roleManager, logger);
    }

    private static async Task SeedCountriesAndCities(ApplicationDatabaseContext dataContext, ILogger<DatabaseInitializer> logger)
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

    private static async Task SeedAdmin(UserManager<IdentityUser> userManager, IConfiguration configuration, ApplicationDatabaseContext dataContext, ILogger<DatabaseInitializer> logger)
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

                City city = await dataContext.Cities
                    .OrderBy(c => c.Id)
                    .FirstOrDefaultAsync() ?? throw new Exception("No cities found in the database. Please ensure that countries and cities are seeded before seeding the admin user.");

                ApplicationUser adminAppUser = new("Admin", "Admin", DateTime.UtcNow.AddYears(-25), Gender.Male, city.Id);

                await CreateUser(admin, adminAppUser, configuration["AdminCred:Password"]!, [RolesNameValues.Admin, RolesNameValues.User, RolesNameValues.Moderator], userManager, dataContext);
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
                List<IdentityRole> roles =
                [
                    new() {Name = RolesNameValues.Admin},
                    new() {Name = RolesNameValues.Moderator},
                    new() {Name = RolesNameValues.User}
                ];

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

    private static async Task AddTestUsers(UserManager<IdentityUser> userManager, ApplicationDatabaseContext dataContext, IConfiguration configuration, ILogger<DatabaseInitializer> logger)
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

                // Generate application user
                var testApplicationUser = new Faker<ApplicationUser>()
                    .CustomInstantiator(f =>
                    {
                        var gender = f.PickRandom<Gender>();

                        var user = new ApplicationUser(f.Name.FirstName((gender == Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female),
                            f.Name.LastName(Bogus.DataSets.Name.Gender.Male),
                            f.Date.Past(refDate: DateTime.UtcNow.AddYears(-18), yearsToGoBack: 70),
                            gender,
                            f.PickRandom(cities).Id);
                        return user;
                    });

                var applicationUser = testApplicationUser.Generate();

                await CreateUser(identityUser, applicationUser, "Pwd12345", [RolesNameValues.User], userManager, dataContext);

                // Add some posts to that user
                await AddPostsToUser(dataContext, random, applicationUser);
            }

            // add all users as friends to user1
            if (dataContext.Friends.Any()) return;
            IdentityUser? firstIdentityUser = await userManager.Users.Where(u => u.Email == "user1@test").FirstOrDefaultAsync();

            List<ApplicationUser> applicationUsers = await dataContext.ApplicationUsers
                .Take(100).
                ToListAsync();

            ApplicationUser? firstUser = applicationUsers.Where(u => u.IdentityId == firstIdentityUser?.Id).FirstOrDefault();

            if (firstUser is not null)
            {
                List<Friend> friends = [];
                foreach (var user in applicationUsers)
                {
                    if (user.IdentityId != firstIdentityUser?.Id)
                    {
                        Friend fr = Friend.CreateFromAcceptedRequest(firstUser.Id, user.Id);
                        friends.Add(fr);
                    }
                }

                dataContext.Friends.AddRange(friends);
                await dataContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding test users.");
        }
    }

    private static async Task AddPostsToUser(ApplicationDatabaseContext dataContext, Random random, ApplicationUser applicationUser)
    {
        Faker<Post> postsFaker = new Faker<Post>()
                            .RuleFor(p => p.UserId, f => applicationUser.Id)
                            .RuleFor(p => p.Content, f => f.Lorem.Paragraph())
                            .RuleFor(p => p.DatePosted, f => f.Date.Past(3))
                            .RuleFor(p => p.DateEdited, f => f.Date.Recent(30));

        int randomNumber = random.Next(0, 101);
        List<Post> fakePosts = postsFaker.Generate(randomNumber);

        dataContext.Posts.AddRange(fakePosts);
        await dataContext.SaveChangesAsync();
    }

    private static async Task CreateUser(IdentityUser identityUser, ApplicationUser appUser, string password, List<string> roles, UserManager<IdentityUser> userManager, ApplicationDatabaseContext dataContext)
    {
        await userManager.CreateAsync(identityUser, password);
        foreach(var role in roles)
        {
            await userManager.AddToRoleAsync(identityUser, role);
        }

        appUser.AssociateWithIdentity(identityUser.Id);

        await dataContext.ApplicationUsers.AddAsync(appUser);
        await dataContext.SaveChangesAsync();
    }
}