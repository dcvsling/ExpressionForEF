using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using ExpressionForEF.Tests;
using System;
using System.Linq;

namespace ExpressionForEF.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ExpressionBenchmark>();
        }
    }

    [BenchmarkDotNet.Attributes.AllCategoriesFilter, AllStatisticsColumn, MemoryDiagnoser]
    public class ExpressionBenchmark
    {
        [GlobalSetup]
        public void SetupOptions()
        {
            Options = OptionsHelper.CreateOptions<TestMaster>(TestData.QueryValues);
        }

        public QueryOptions<TestMaster> Options { get; set; }
        [Benchmark]
        public void UseHelper()
            => new TestDbContext().TestMasters.ConfigureQuery(Options);

        [Benchmark]
        public void Directly()
            => new TestDbContext().TestMasters
                .Where(t => t.No >= 1 && t.No <= 3)
                .Where(t => t.Name == "A" || t.Name == "B" || t.Name == "D")
                .Where(t => t.ModityDate != DateTimeOffset.MaxValue)
                .Where(t => t.Id != Guid.Empty)
                .OrderByDescending(t => t.No)
                .ThenBy(t => t.ModityDate)
                .Skip(3)
                .Take(3);
    }
}
