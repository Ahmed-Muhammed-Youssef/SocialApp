using System.Diagnostics;
using Bogus;
using Domain.ApplicationUserAggregate;
using Domain.FriendAggregate;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Shared.Constants;

namespace SeedDB.Seeding;

/// <summary>
/// Runs the seeding phases in order: reset, users, friendships, posts.
/// </summary>
/// <remarks>
/// The phases are plain sequential methods rather than a pluggable pipeline: the user ids that
/// friendships and posts need travel as a return value and a parameter, so the compiler enforces
/// the ordering.
/// </remarks>
internal sealed partial class DatabaseSeeder(IServiceProvider services, ILogger<DatabaseSeeder> logger)
{
    /// <summary>Every seeded login gets this domain, so --reset can find its own data and nothing else.</summary>
    private const string SeedEmailDomain = "@seed.local";

    /// <summary>SQL LIKE form of <see cref="SeedEmailDomain"/>, used instead of EndsWith so EF can translate it.</summary>
    private const string SeedEmailPattern = "%@seed.local";

    /// <summary>Shared password for every seeded login. Matches the API's relaxed password policy.</summary>
    private const string SeedPassword = "Pwd12345";

    /// <summary>Rows per SaveChanges. Keeps the change tracker small on large runs.</summary>
    private const int BatchSize = 500;

    /// <summary>Makes this run's emails unique, so seeding twice without --reset does not collide.</summary>
    private readonly string _runTag = Guid.NewGuid().ToString("N")[..8];

    private readonly Faker _faker = new();

    public async Task<SeedReport> RunAsync(SeedOptions options, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(options);

        List<SeedPhaseResult> phases = [];
        long runStartedAt = Stopwatch.GetTimestamp();

        if (options.Reset)
        {
            long resetStartedAt = BeginPhase("reset");
            int deleted = await ResetAsync(cancellationToken);
            phases.Add(EndPhase("reset", deleted, resetStartedAt));
        }
        else
        {
            phases.Add(SeedPhaseResult.AsSkipped("reset"));
        }

        long usersStartedAt = BeginPhase("users");
        IReadOnlyList<int> userIds = await SeedUsersAsync(options, cancellationToken);
        phases.Add(EndPhase("users", userIds.Count, usersStartedAt));

        long friendshipsStartedAt = BeginPhase("friendships");
        int friendships = await SeedFriendshipsAsync(userIds, options, cancellationToken);
        phases.Add(EndPhase("friendships", friendships, friendshipsStartedAt));

        long postsStartedAt = BeginPhase("posts");
        int posts = await SeedPostsAsync(userIds, options, cancellationToken);
        phases.Add(EndPhase("posts", posts, postsStartedAt));

        return new SeedReport(phases, Stopwatch.GetElapsedTime(runStartedAt));
    }

    /// <summary>Deletes data from earlier seed runs, identified by the seed email domain.</summary>
    private async Task<int> ResetAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = CreateScope();
        ApplicationDatabaseContext context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();

        IQueryable<int> seededUserIds = context.ApplicationUsers
            .Where(user => context.Users.Any(login =>
                login.Id == user.IdentityId && EF.Functions.Like(login.Email!, SeedEmailPattern)))
            .Select(user => user.Id);

        // Friend rows are configured as ClientCascade, so they must go before the users they
        // point at. Logins go last: the query above joins through them to find the users.
        int posts = await context.Posts
            .Where(post => seededUserIds.Contains(post.UserId))
            .ExecuteDeleteAsync(cancellationToken);

        int friends = await context.Friends
            .Where(friend => seededUserIds.Contains(friend.UserId) || seededUserIds.Contains(friend.FriendId))
            .ExecuteDeleteAsync(cancellationToken);

        int users = await context.ApplicationUsers
            .Where(user => seededUserIds.Contains(user.Id))
            .ExecuteDeleteAsync(cancellationToken);

        int logins = await context.Users
            .Where(login => EF.Functions.Like(login.Email!, SeedEmailPattern))
            .ExecuteDeleteAsync(cancellationToken);

        return posts + friends + users + logins;
    }

    /// <summary>
    /// Creates the users with their Identity logins and returns the domain ids
    /// (<c>ApplicationUser.Id</c>, not <c>IdentityId</c>) in creation order.
    /// </summary>
    private async Task<IReadOnlyList<int>> SeedUsersAsync(SeedOptions options, CancellationToken cancellationToken)
    {
        if (options.Users <= 0)
        {
            return [];
        }

        (List<int> cityIds, string roleId) = await ReadUserPrerequisitesAsync(cancellationToken);
        List<int> userIds = new(options.Users);

        for (int start = 0; start < options.Users; start += BatchSize)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int batchSize = Math.Min(BatchSize, options.Users - start);

            using IServiceScope scope = CreateScope();
            ApplicationDatabaseContext context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
            ILookupNormalizer normalizer = scope.ServiceProvider.GetRequiredService<ILookupNormalizer>();

            // Hashing is the expensive part of creating a login, and every seeded user shares the
            // same password, so it is hashed once per batch rather than once per user.
            string passwordHash = scope.ServiceProvider
                .GetRequiredService<IPasswordHasher<IdentityUser>>()
                .HashPassword(new IdentityUser(), SeedPassword);

            List<ApplicationUser> batch = new(batchSize);

            for (int offset = 0; offset < batchSize; offset++)
            {
                string email = $"seed.{start + offset}.{_runTag}{SeedEmailDomain}";

                // Written straight to the tables rather than through UserManager: UserManager
                // saves and validates per user, which would be two round trips each.
                IdentityUser login = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = email,
                    NormalizedUserName = normalizer.NormalizeName(email),
                    Email = email,
                    NormalizedEmail = normalizer.NormalizeEmail(email),
                    EmailConfirmed = true,
                    PasswordHash = passwordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                };

                context.Users.Add(login);
                context.UserRoles.Add(new IdentityUserRole<string> { UserId = login.Id, RoleId = roleId });
                batch.Add(CreateUser(login.Id, cityIds));
            }

            context.ApplicationUsers.AddRange(batch);
            await context.SaveChangesAsync(cancellationToken);

            // Ids are database-generated, so they are only known after the save.
            userIds.AddRange(batch.Select(user => user.Id));
        }

        return userIds;
    }

    private ApplicationUser CreateUser(string identityId, List<int> cityIds)
    {
        Gender gender = _faker.PickRandom<Gender>();

        ApplicationUser user = new(
            _faker.Name.FirstName(gender == Gender.Male ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female),
            _faker.Name.LastName(),
            _faker.Date.Past(60, DateTime.UtcNow.AddYears(-18)),
            gender,
            _faker.PickRandom(cityIds));

        user.AssociateWithIdentity(identityId);

        return user;
    }

    private async Task<(List<int> CityIds, string RoleId)> ReadUserPrerequisitesAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = CreateScope();
        ApplicationDatabaseContext context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();

        List<int> cityIds = await context.Cities.Select(city => city.Id).ToListAsync(cancellationToken);
        IdentityRole role = await context.Roles.FirstAsync(r => r.Name == RolesNameValues.User, cancellationToken);

        return (cityIds, role.Id);
    }

    /// <summary>
    /// Gives each user <see cref="SeedOptions.FriendsPerUser"/> friendships by pairing it with the
    /// next users in the list, wrapping at the end. Pairs are deduplicated because
    /// <see cref="Friend"/> orders its two ids and uses them as a composite key.
    /// </summary>
    private async Task<int> SeedFriendshipsAsync(IReadOnlyList<int> userIds, SeedOptions options, CancellationToken cancellationToken)
    {
        if (userIds.Count < 2 || options.FriendsPerUser <= 0)
        {
            return 0;
        }

        int friendsPerUser = Math.Min(options.FriendsPerUser, userIds.Count - 1);
        HashSet<(int First, int Second)> pairs = [];
        List<Friend> batch = new(BatchSize);
        int written = 0;

        for (int index = 0; index < userIds.Count; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            for (int offset = 1; offset <= friendsPerUser; offset++)
            {
                int userId = userIds[index];
                int friendId = userIds[(index + offset) % userIds.Count];

                if (!pairs.Add(userId < friendId ? (userId, friendId) : (friendId, userId)))
                {
                    continue;
                }

                batch.Add(Friend.CreateFromAcceptedRequest(userId, friendId));

                if (batch.Count == BatchSize)
                {
                    written += await FlushAsync(batch, cancellationToken);
                }
            }
        }

        return written + await FlushAsync(batch, cancellationToken);
    }

    /// <summary>Attaches <see cref="SeedOptions.PostsPerUser"/> posts to each seeded user.</summary>
    private async Task<int> SeedPostsAsync(IReadOnlyList<int> userIds, SeedOptions options, CancellationToken cancellationToken)
    {
        if (userIds.Count == 0 || options.PostsPerUser <= 0)
        {
            return 0;
        }

        List<Post> batch = new(BatchSize);
        int written = 0;

        foreach (int userId in userIds)
        {
            cancellationToken.ThrowIfCancellationRequested();

            for (int index = 0; index < options.PostsPerUser; index++)
            {
                batch.Add(Post.Create(userId, _faker.Lorem.Paragraph()));

                if (batch.Count == BatchSize)
                {
                    written += await FlushAsync(batch, cancellationToken);
                }
            }
        }

        return written + await FlushAsync(batch, cancellationToken);
    }

    /// <summary>Inserts a batch in its own scope and empties the list. Returns rows written.</summary>
    private async Task<int> FlushAsync<TEntity>(List<TEntity> entities, CancellationToken cancellationToken)
        where TEntity : class
    {
        if (entities.Count == 0)
        {
            return 0;
        }

        using IServiceScope scope = CreateScope();
        ApplicationDatabaseContext context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();

        context.Set<TEntity>().AddRange(entities);
        await context.SaveChangesAsync(cancellationToken);

        int written = entities.Count;
        entities.Clear();

        return written;
    }

    /// <summary>
    /// A scope per batch: the EF change tracker grows with everything it has seen, so one scope
    /// across a large run makes each SaveChanges progressively slower.
    /// </summary>
    private IServiceScope CreateScope() => services.CreateScope();

    private long BeginPhase(string phase)
    {
        Log.RunningPhase(logger, phase);
        return Stopwatch.GetTimestamp();
    }

    private SeedPhaseResult EndPhase(string phase, int written, long startedAt)
    {
        TimeSpan elapsed = Stopwatch.GetElapsedTime(startedAt);
        Log.PhaseCompleted(logger, phase, written, elapsed);
        return new SeedPhaseResult(phase, written, elapsed, Skipped: false);
    }

    /// <summary>Source-generated log methods, required by CA1873 when logging a value type.</summary>
    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Running {Phase}")]
        public static partial void RunningPhase(ILogger logger, string phase);

        [LoggerMessage(Level = LogLevel.Information, Message = "{Phase} wrote {Records} records in {Elapsed}")]
        public static partial void PhaseCompleted(ILogger logger, string phase, int records, TimeSpan elapsed);
    }
}
