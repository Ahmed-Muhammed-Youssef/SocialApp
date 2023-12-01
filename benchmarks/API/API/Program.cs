using API.Benchmark.Controllers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace API.Benchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new MyBenchmarkConfig();
            BenchmarkRunner.Run<UsersControllerBenchmark>(config);
        }
    }
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
        }
    }
}
