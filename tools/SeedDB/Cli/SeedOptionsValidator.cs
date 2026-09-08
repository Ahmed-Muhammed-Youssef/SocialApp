namespace SeedDB.Cli;

/// <summary>
/// Semantic validation: the arguments parsed cleanly, but do the values describe a run worth doing?
/// Errors are appended rather than thrown so the caller sees every problem in one pass. This runs
/// before any database connection is opened, so it is the last gate before a destructive
/// <c>--reset</c> reaches a real database.
/// </summary>
internal static class SeedOptionsValidator
{
    public static void Validate(SeedOptions options, ICollection<string> errors)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(errors);

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            errors.Add("--connection must not be blank.");
        }
    }
}
