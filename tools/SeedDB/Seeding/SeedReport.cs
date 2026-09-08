using System.Globalization;

namespace SeedDB.Seeding;

/// <summary>What one phase did, for the run summary.</summary>
/// <param name="Phase">Phase name.</param>
/// <param name="RecordsWritten">Records the phase wrote.</param>
/// <param name="Elapsed">Wall-clock time the phase took.</param>
/// <param name="Skipped">True when the phase did not apply to this run.</param>
internal sealed record SeedPhaseResult(string Phase, int RecordsWritten, TimeSpan Elapsed, bool Skipped)
{
    public static SeedPhaseResult AsSkipped(string phase) => new(phase, 0, TimeSpan.Zero, true);
}

/// <summary>The outcome of a whole seeding run.</summary>
/// <param name="Phases">Per-phase results, in execution order.</param>
/// <param name="Elapsed">Wall-clock time for the whole run.</param>
internal sealed record SeedReport(IReadOnlyList<SeedPhaseResult> Phases, TimeSpan Elapsed)
{
    public int TotalRecordsWritten => Phases.Sum(phase => phase.RecordsWritten);

    public string ToSummary()
    {
        IEnumerable<string> lines = Phases.Select(phase => phase.Skipped
            ? $"  {phase.Phase}: skipped"
            : string.Create(
                CultureInfo.InvariantCulture,
                $"  {phase.Phase}: {phase.RecordsWritten} records in {phase.Elapsed.TotalSeconds:F1}s"));

        string total = string.Create(
            CultureInfo.InvariantCulture,
            $"Seeded {TotalRecordsWritten} records in {Elapsed.TotalSeconds:F1}s");

        return string.Join(Environment.NewLine, [total, .. lines]);
    }
}
