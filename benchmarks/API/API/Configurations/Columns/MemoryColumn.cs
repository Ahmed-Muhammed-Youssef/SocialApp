using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace API.Benchmark.Configurations.Columns
{
    public class MemoryColumn : IColumn
    {
        public string Id => nameof(MemoryColumn);
        public string ColumnName { get; } = "Memory";
        public bool AlwaysShow => true;
        public int PriorityInCategory => 1;
        public bool IsNumeric => true;
        public UnitType UnitType => UnitType.Size;

        public string Legend => "Allocated memory per operation";

        public ColumnCategory Category => ColumnCategory.Custom;

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
        {
            var gcStats = summary[benchmarkCase].GcStats;

            // Use GetBytesAllocatedPerOperation method
            var bytesAllocatedPerOperation = gcStats.GetBytesAllocatedPerOperation(benchmarkCase);

            // You can return any memory-related statistic here. For example:
            return bytesAllocatedPerOperation.HasValue
                ? $"{bytesAllocatedPerOperation.Value / (1024.0 * 1024):F2} MB"
                : "N/A";
        }
        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        {
            return GetValue(summary, benchmarkCase);
        }

        public bool IsAvailable(Summary summary) => true;
        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
    }
}
