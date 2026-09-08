namespace SeedDB;

/// <summary>
/// The validated inputs for a seeding run. Produced by <c>SeedDB.Cli</c> and consumed by
/// <c>SeedDB.Seeding</c>, so it deliberately knows nothing about either.
/// </summary>
/// <param name="ConnectionString">SQL Server connection string of the database to seed.</param>
/// <param name="Users">Number of users to seed.</param>
/// <param name="FriendsPerUser">Number of accepted friendships to create for each seeded user.</param>
/// <param name="PostsPerUser">Number of posts to create for each seeded user.</param>
/// <param name="Reset">Whether previously seeded data is deleted before seeding.</param>
internal sealed record SeedOptions(
    string ConnectionString,
    int Users,
    int FriendsPerUser,
    int PostsPerUser,
    bool Reset)
{
    /// <summary>Default user count, large enough for newsfeed and paging queries to be meaningful.</summary>
    public const int DefaultUsers = 100;

    /// <summary>Default friendships per user.</summary>
    public const int DefaultFriendsPerUser = 10;

    /// <summary>Default posts per user.</summary>
    public const int DefaultPostsPerUser = 10;

    /// <summary>
    /// Renders the options for logging with any password removed, so a run can be traced without
    /// writing credentials into a console log or CI output.
    /// </summary>
    public string ToSafeSummary() =>
        $"users={Users}, friends-per-user={FriendsPerUser}, posts-per-user={PostsPerUser}, " +
        $"reset={Reset}, connection={RedactSecrets(ConnectionString)}";

    private static string RedactSecrets(string connectionString)
    {
        IEnumerable<string> parts = connectionString
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(part => IsSecret(part) ? $"{part[..part.IndexOf('=', StringComparison.Ordinal)]}=***" : part);

        return string.Join(';', parts);
    }

    private static bool IsSecret(string part) =>
        part.Contains('=', StringComparison.Ordinal)
        && (part.StartsWith("Password", StringComparison.OrdinalIgnoreCase)
            || part.StartsWith("Pwd", StringComparison.OrdinalIgnoreCase));
}
