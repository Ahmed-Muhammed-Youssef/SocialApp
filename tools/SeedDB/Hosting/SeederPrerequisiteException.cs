namespace SeedDB.Hosting;

/// <summary>
/// Thrown when the target database is not in a state that can be seeded. Carries a message written
/// for the person running the command, so <c>Program</c> can print it as-is and exit.
/// </summary>
internal sealed class SeederPrerequisiteException : Exception
{
    public SeederPrerequisiteException(string message) : base(message)
    {
    }

    public SeederPrerequisiteException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public SeederPrerequisiteException()
    {
    }
}
