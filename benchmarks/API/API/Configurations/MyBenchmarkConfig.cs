using API.Benchmark.Configurations.Columns;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace API.Benchmark.Configurations
{
    public class MyBenchmarkConfig : ManualConfig
    {
        public MyBenchmarkConfig()
        {
            AddJob(Job.Default.WithToolchain(InProcessEmitToolchain.Instance)); // Use InProcessToolchain
            AddLogger(ConsoleLogger.Default); // Attach ConsoleLogger
            AddDiagnoser(MemoryDiagnoser.Default);
            AddColumn(TargetMethodColumn.Method);
            AddColumn(StatisticColumn.Max);
            AddColumn(StatisticColumn.Min);
            AddColumn(StatisticColumn.StdDev);
            AddColumn(StatisticColumn.Mean);
            AddColumn(StatisticColumn.Median);
            AddColumn(StatisticColumn.OperationsPerSecond);
            AddColumn(new MemoryColumn()); // Add custom memory column
        }
    }
}
