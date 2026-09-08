using System.Globalization;

namespace SeedDB.Cli;

/// <summary>
/// Reads the seeder's arguments. This layer only handles syntax — unknown flags, repeated flags,
/// missing values and non-numeric numbers. Rules about which combinations of values make sense
/// belong in <see cref="SeedOptionsValidator"/>.
/// </summary>
internal static class CommandLineParser
{
    private const string ConnectionFlag = "--connection";
    private const string UsersFlag = "--users";
    private const string FriendsPerUserFlag = "--friends-per-user";
    private const string PostsPerUserFlag = "--posts-per-user";
    private const string ResetFlag = "--reset";

    public static string Usage =>
        $"""
        Seeds the SocialApp database with generated users, friendships and posts.

        Usage:
          dotnet run --project tools/SeedDB -- --connection <connection-string> [options]

        Options:
          {ConnectionFlag} <value>        SQL Server connection string of the database to seed. Required.
          {UsersFlag} <n>                 Number of users to seed. Default: {SeedOptions.DefaultUsers}.
          {FriendsPerUserFlag} <n>      Friendships created per seeded user. Default: {SeedOptions.DefaultFriendsPerUser}.
          {PostsPerUserFlag} <n>        Posts created per seeded user. Default: {SeedOptions.DefaultPostsPerUser}.
          {ResetFlag}                     Delete existing seeded data before seeding. Destructive.
          -h, --help                  Show this help text.

        Both '--flag value' and '--flag=value' forms are accepted. The target database must already
        have its schema and reference data (roles, cities) in place — run the API once, or apply
        scripts/seed_essential_data.sql, before seeding.
        """;

    public static ParseOutcome Parse(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        List<string> errors = [];
        HashSet<string> seenFlags = new(StringComparer.Ordinal);

        string? connectionString = null;
        int? users = null;
        int? friendsPerUser = null;
        int? postsPerUser = null;
        bool reset = false;

        for (int index = 0; index < args.Length; index++)
        {
            string token = args[index];

            if (token is "-h" or "--help" or "-?")
            {
                return ParseOutcome.Help();
            }

            if (!token.StartsWith("--", StringComparison.Ordinal))
            {
                errors.Add($"Unexpected argument '{token}'. Every input must be passed as a flag.");
                continue;
            }

            // Split on the FIRST '=' only: a connection string value legitimately contains more of them.
            int separator = token.IndexOf('=', StringComparison.Ordinal);
            string flag = separator >= 0 ? token[..separator] : token;
            string? inlineValue = separator >= 0 ? token[(separator + 1)..] : null;

            if (!seenFlags.Add(flag))
            {
                errors.Add($"Flag '{flag}' was specified more than once.");
                continue;
            }

            switch (flag)
            {
                case ResetFlag:
                    if (inlineValue is not null)
                    {
                        errors.Add($"Flag '{ResetFlag}' is a switch and takes no value.");
                    }

                    reset = true;
                    break;

                case ConnectionFlag:
                    connectionString = ReadValue(args, ref index, flag, inlineValue, errors);
                    break;

                case UsersFlag:
                    users = ReadInt32(args, ref index, flag, inlineValue, errors);
                    break;

                case FriendsPerUserFlag:
                    friendsPerUser = ReadInt32(args, ref index, flag, inlineValue, errors);
                    break;

                case PostsPerUserFlag:
                    postsPerUser = ReadInt32(args, ref index, flag, inlineValue, errors);
                    break;

                default:
                    errors.Add($"Unknown flag '{flag}'.");
                    break;
            }
        }

        if (connectionString is null && !seenFlags.Contains(ConnectionFlag))
        {
            errors.Add($"Flag '{ConnectionFlag}' is required — the seeder never guesses which database to write to.");
        }

        if (errors.Count > 0)
        {
            return ParseOutcome.Invalid(errors);
        }

        SeedOptions options = new(
            connectionString!,
            users ?? SeedOptions.DefaultUsers,
            friendsPerUser ?? SeedOptions.DefaultFriendsPerUser,
            postsPerUser ?? SeedOptions.DefaultPostsPerUser,
            reset);

        SeedOptionsValidator.Validate(options, errors);

        return errors.Count > 0 ? ParseOutcome.Invalid(errors) : ParseOutcome.Valid(options);
    }

    /// <summary>
    /// Takes the flag's value from either '--flag=value' or the following argument. A following
    /// argument that itself looks like a flag is treated as a missing value rather than consumed,
    /// so '--connection --users 10' reports the real mistake instead of seeding against "--users".
    /// </summary>
    private static string? ReadValue(string[] args, ref int index, string flag, string? inlineValue, List<string> errors)
    {
        if (inlineValue is not null)
        {
            if (inlineValue.Length == 0)
            {
                errors.Add($"Flag '{flag}' was given an empty value.");
                return null;
            }

            return inlineValue;
        }

        if (index + 1 >= args.Length || args[index + 1].StartsWith("--", StringComparison.Ordinal))
        {
            errors.Add($"Flag '{flag}' requires a value.");
            return null;
        }

        index++;
        return args[index];
    }

    private static int? ReadInt32(string[] args, ref int index, string flag, string? inlineValue, List<string> errors)
    {
        string? value = ReadValue(args, ref index, flag, inlineValue, errors);

        if (value is null)
        {
            return null;
        }

        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
        {
            errors.Add($"Flag '{flag}' expects a whole number but got '{value}'.");
            return null;
        }

        return parsed;
    }
}
