using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using System.Globalization;
using ZipService.Api.Benchmarks;

var config = new ManualConfig()
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub)
    .AddValidator(JitOptimizationsValidator.DontFailOnError)
    .AddLogger(ConsoleLogger.Default)
    .AddColumnProvider(DefaultColumnProviders.Instance);

config.SummaryStyle =
    new SummaryStyle(
        CultureInfo.CurrentCulture,
        true,
        null,
        Perfolizer.Horology.TimeUnit.Second);

BenchmarkRunner.Run<ZipBenchmark>(config);