using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;

namespace API.Benchmark.Configurations;

/// <summary>
/// Configuration tuned for database-backed benchmarks rather than nanosecond-scale microbenchmarks.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="RunStrategy.Monitoring"/> with an invocation count of one runs a fixed number of single-call
/// iterations. The default strategy's pilot stage would instead batch many calls into one measurement,
/// which is right for a pure function and wrong for a millisecond-scale round trip to SQL Server — and it
/// would also break the per-iteration transaction and scope handling the benchmarks rely on.
/// </para>
/// <para>
/// The default out-of-process toolchain is used deliberately. The previous configuration used
/// <c>InProcessEmitToolchain</c>, which shares a process, GC and JIT state with the harness; it is faster
/// to start but less trustworthy, and it cannot validate that the code under test was built optimized.
/// </para>
/// <para>
/// P95 is reported alongside the mean because service performance is judged on tail latency, and results
/// are exported to <c>BenchmarkDotNet.Artifacts/</c> so a run can be committed and diffed instead of being
/// transcribed into a source comment.
/// </para>
/// </remarks>
public class MyBenchmarkConfig : ManualConfig
{
    public MyBenchmarkConfig()
    {
        AddJob(Job.Default
            .WithStrategy(RunStrategy.Monitoring)
            .WithInvocationCount(1)
            .WithUnrollFactor(1)
            .WithWarmupCount(3)
            .WithIterationCount(20));

        AddLogger(ConsoleLogger.Default);
        AddDiagnoser(MemoryDiagnoser.Default);

        AddColumnProvider(DefaultColumnProviders.Instance);
        AddColumn(StatisticColumn.Min);
        AddColumn(StatisticColumn.Max);
        AddColumn(StatisticColumn.P95);
        AddColumn(StatisticColumn.OperationsPerSecond);

        AddExporter(MarkdownExporter.GitHub);
        AddExporter(CsvExporter.Default);
    }
}
