using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Helpers;

namespace Helpers.Benchmarks;

[MemoryDiagnoser]
public class StringHelperBenchmarks
{
    private const string Input = "Hello World! This is a test string with 123 numbers and some symbols @#$.";

    [Benchmark]
    public bool IsAlpha() => StringHelper.IsAlpha("HelloWorld");

    [Benchmark]
    public bool IsAlphaNumeric() => StringHelper.IsAlphaNumeric("Hello123");

    [Benchmark]
    public string RemoveExtraSpaces() => "  Hello   World  ".RemoveExtraSpaces();

    [Benchmark]
    public string ToSnakeCase() => "HelloWorldTest".ToSnakeCase();
}

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<StringHelperBenchmarks>();
    }
}
