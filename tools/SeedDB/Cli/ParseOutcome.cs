using System.Diagnostics.CodeAnalysis;

namespace SeedDB.Cli;

/// <summary>
/// The result of reading the command line: either options to seed with, a request for help,
/// or the complete list of problems found. Every problem is reported at once rather than
/// failing on the first one, so a caller fixing their command line only has to run once more.
/// </summary>
internal sealed record ParseOutcome
{
    private ParseOutcome() { }

    public SeedOptions? Options { get; private init; }

    public IReadOnlyList<string> Errors { get; private init; } = [];

    public bool HelpRequested { get; private init; }

    [MemberNotNullWhen(true, nameof(Options))]
    public bool IsValid => Options is not null && Errors.Count == 0;

    public static ParseOutcome Help() => new() { HelpRequested = true };

    public static ParseOutcome Valid(SeedOptions options) => new() { Options = options };

    public static ParseOutcome Invalid(IReadOnlyList<string> errors) => new() { Errors = errors };
}
