using API.Benchmark.Configurations;
using BenchmarkDotNet.Running;

namespace API.Benchmark;

public static class Program
{
    /// <summary>
    /// Runs via <see cref="BenchmarkSwitcher"/> so a single class can be selected from the command line
    /// instead of every benchmark running on each invocation:
    /// <code>
    /// dotnet run -c Release --project benchmarks/API/API -- --filter *PostQueriesBenchmark*
    /// </code>
    /// </summary>
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new MyBenchmarkConfig());
    }
}
