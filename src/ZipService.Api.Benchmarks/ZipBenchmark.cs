using BenchmarkDotNet.Attributes;

namespace ZipService.Api.Benchmarks;

public class ZipBenchmark
{
    private readonly MemoryStream _asyncStream = new();
    private readonly CancellationTokenSource _cts = new();

    [Benchmark]
    public void ZipWrite_With10()
    {
        Zip.Write(_asyncStream, 10);
    }

    [Benchmark]
    public async Task ZipWriteAsync_With10()
    {
        await Zip.WriteAsync(_asyncStream, 10, _cts.Token);
    }

    [Benchmark]
    public void ZipWrite_With50()
    {
        Zip.Write(_asyncStream, 50);
    }

    [Benchmark]
    public async Task ZipWriteAsync_With50()
    {
        await Zip.WriteAsync(_asyncStream, 50, _cts.Token);
    }

    [Benchmark]
    public void ZipWrite_With80()
    {
        Zip.Write(_asyncStream, 50);
    }

    [Benchmark]
    public async Task ZipWriteAsync_With80()
    {
        await Zip.WriteAsync(_asyncStream, 50, _cts.Token);
    }

    [Benchmark]
    public void ZipWrite_With100()
    {
        Zip.Write(_asyncStream, 100);
    }

    [Benchmark]
    public async Task ZipWriteAsync_With100()
    {
        await Zip.WriteAsync(_asyncStream, 1000, _cts.Token);
    }

    [Benchmark]
    public void ZipWrite_With1000()
    {
        Zip.Write(_asyncStream, 100);
    }

    [Benchmark]
    public async Task ZipWriteAsync_With1000()
    {
        await Zip.WriteAsync(_asyncStream, 1000, _cts.Token);
    }
}