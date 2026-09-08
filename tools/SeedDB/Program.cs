using SeedDB.Cli;
using SeedDB.Hosting;
using SeedDB.Seeding;

namespace SeedDB;

internal static class Program
{
    private const int ExitSuccess = 0;
    private const int ExitInvalidArguments = 2;
    private const int ExitPrerequisiteFailed = 3;
    private const int ExitCancelled = 4;

    private static async Task<int> Main(string[] args)
    {
        ParseOutcome outcome = CommandLineParser.Parse(args);

        if (outcome.HelpRequested)
        {
            Console.WriteLine(CommandLineParser.Usage);
            return ExitSuccess;
        }

        if (!outcome.IsValid)
        {
            foreach (string error in outcome.Errors)
            {
                Console.Error.WriteLine($"error: {error}");
            }

            Console.Error.WriteLine();
            Console.Error.WriteLine(CommandLineParser.Usage);
            return ExitInvalidArguments;
        }

        SeedOptions options = outcome.Options;
        Console.WriteLine($"Seeding with {options.ToSafeSummary()}");

        using CancellationTokenSource cancellation = new();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            // Seeding can run for minutes; let Ctrl+C unwind the current phase instead of killing
            // the process mid-transaction.
            eventArgs.Cancel = true;
            cancellation.Cancel();
        };

        await using ServiceProvider provider = SeederHost.CreateServiceProvider(options);

        try
        {
            await SeederHost.EnsurePrerequisitesAsync(provider, cancellation.Token);

            DatabaseSeeder seeder = new(provider, provider.GetRequiredService<ILogger<DatabaseSeeder>>());
            SeedReport report = await seeder.RunAsync(options, cancellation.Token);

            Console.WriteLine(report.ToSummary());
            return ExitSuccess;
        }
        catch (SeederPrerequisiteException ex)
        {
            Console.Error.WriteLine($"error: {ex.Message}");
            return ExitPrerequisiteFailed;
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Seeding cancelled. Data written by completed phases is still in the database.");
            return ExitCancelled;
        }
    }
}
