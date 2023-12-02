using API.Benchmark.Configurations;
using API.Benchmark.Controllers;
using BenchmarkDotNet.Running;

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
    
   
}
