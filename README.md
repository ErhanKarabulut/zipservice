# Zip File Generator in ASP.NET

## Table of Contents
- [Zip File Generator in ASP.NET](#zip-file-generator-in-aspnet)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Features](#features)
  - [Key Improvements](#key-improvements)
    - [Thread Safety in Random Number Generation](#thread-safety-in-random-number-generation)
      - [Original Version](#original-version)
      - [Improved Version](#improved-version)
    - [Asynchronous Programming](#asynchronous-programming)
      - [Original Version](#original-version-1)
      - [Improved Version](#improved-version-1)
    - [Memory Optimization](#memory-optimization)
      - [Original Version](#original-version-2)
      - [Improved Version](#improved-version-2)
    - [Compression Level](#compression-level)
      - [Original Version](#original-version-3)
      - [Improved Version](#improved-version-3)
    - [Request Cancellation](#request-cancellation)
      - [Original Version](#original-version-4)
      - [Improved Version](#improved-version-4)
  - [Testing and Benchmarking](#testing-and-benchmarking)
    - [Automated Testing](#automated-testing)
    - [Manual Testing](#manual-testing)
    - [Performance Benchmarking](#performance-benchmarking)
    - [Metrics to Measure](#metrics-to-measure)
    - [Data Analysis](#data-analysis)
    - [Continuous Monitoring](#continuous-monitoring)
  - [Conclusion](#conclusion)

## Overview

This ASP.NET project creates a minimal web API to serve ZIP files on the fly. The ZIP files contain a given number of files, each being 1MiB in size and filled with random data. The project aims to focus on scalability, performance, and low memory consumption.

## Features

- Generates ZIP files with `n` number of 1MiB files filled with random data.
- Asynchronous operation support.
- Request cancellation capability.
- Stream-based architecture for low memory footprint.

## Key Improvements

### Thread Safety in Random Number Generation

#### Original Version
- Utilized `System.Random`.
- Initialized with a fixed seed: `var seed = 123; var rnd = new Random(seed);`

**Technical Issues**
- **Thread Safety**: `System.Random` is not thread-safe. If accessed by multiple threads, it can cause lock contention or generate non-random values.
- **Predictability**: The use of a fixed seed makes the random data predictable across multiple runs.

#### Improved Version
- Utilizes `RandomNumberGenerator.Fill`.

**Technical Benefits**
- **Thread-Safe**: `RandomNumberGenerator.Fill` is inherently thread-safe, mitigating all issues related to concurrent accesses.
- **Cryptographically Secure**: Provides a more secure source of randomness than `System.Random`, making it less predictable and more suitable for secure contexts.

---

### Asynchronous Programming

#### Original Version
- Used synchronous I/O operations: `entryStream.Write(buffer);`

**Technical Issues**
- **Thread Blocking**: Could potentially block the web server threads, leading to poor resource utilization.

#### Improved Version
- Uses `async/await`: `await entryStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);`

**Technical Benefits**
- **Non-Blocking**: Utilizes the Task-based Asynchronous Pattern (TAP) to free up the thread for other tasks while waiting for I/O to complete.

---

### Memory Optimization

#### Original Version
- Single buffer of 1MiB per file.

**Technical Issues**
- **High Memory Consumption**: Consumes 1MiB of memory per file, which can be problematic for a high number of large files.

#### Improved Version
- Uses a smaller buffer of 8KiB.

**Technical Benefits**
- **Lower Memory Footprint**: The reduced buffer size and the piecemeal writing approach help to minimize the memory footprint.

---

### Compression Level

#### Original Version
- Default compression level.

**Technical Issues**
- **Suboptimal Compression**: Could lead to larger-than-necessary output files.

#### Improved Version
- Explicitly sets to `CompressionLevel.Optimal`.

**Technical Benefits**
- **Balanced Output**: Achieves a good balance between the size of the compressed data and the time it takes to perform the compression.

---

### Request Cancellation

#### Original Version
- No cancellation support.

**Technical Issues**
- **Resource Hogging**: Ongoing large requests that are no longer needed still continue to execution, leading to resource wastage.

#### Improved Version
- Implements `CancellationToken`.

**Technical Benefits**
- **Graceful Termination**: Allows for the immediate release of server resources if the client cancels the request, improving system resilience and user experience.

## Testing and Benchmarking

```

BenchmarkDotNet v0.13.7, Windows 10 (10.0.19045.3324/22H2/2022Update)
11th Gen Intel Core i7-11850H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.400
  [Host]     : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2 DEBUG [AttachedDebugger]
  DefaultJob : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2


```
|                 Method | Mean [s] | Error [s] | StdDev [s] | Median [s] |
|----------------------- |---------:|----------:|-----------:|-----------:|
|        ZipWrite_With10 | 0.3436 s |  0.0068 s |   0.0123 s |   0.3382 s |
|   ZipWriteAsync_With10 | 0.0086 s |  0.0002 s |   0.0002 s |   0.0086 s |
|        ZipWrite_With50 | 1.7236 s |  0.0342 s |   0.0480 s |   1.7234 s |
|   ZipWriteAsync_With50 | 0.0435 s |  0.0009 s |   0.0010 s |   0.0429 s |
|        ZipWrite_With80 |       NA |        NA |         NA |         NA |
|   ZipWriteAsync_With80 | 0.0700 s |  0.0014 s |   0.0016 s |   0.0697 s |
|       ZipWrite_With100 |       NA |        NA |         NA |         NA |
|  ZipWriteAsync_With100 | 0.0870 s |  0.0005 s |   0.0004 s |   0.0871 s |
|      ZipWrite_With1000 |       NA |        NA |         NA |         NA |
| ZipWriteAsync_With1000 | 0.8712 s |  0.0111 s |   0.0087 s |   0.8673 s |

Benchmarks with issues:
  ZipBenchmark.ZipWrite_With80: DefaultJob
  ZipBenchmark.ZipWrite_With100: DefaultJob
  ZipBenchmark.ZipWrite_With1000: DefaultJob

### Automated Testing

1. **Unit Testing**
   - To ensure that each component works as expected.
   - Libraries: xUnit, NUnit, or MSTest.
   - Test Case Examples:
      - Create a ZIP with 0 files.
      - Create a ZIP with 10 files.
      - Create a ZIP and cancel the request halfway.

2. **Integration Testing**
   - Test the API endpoints to ensure they interact properly with the `Zip` class.
   - Libraries: Postman, RestSharp, or `TestServer` in ASP.NET.

### Manual Testing

1. **Browser Testing**
   - To ensure the API works across different web browsers.

2. **Cross-Platform Testing**
   - To ensure compatibility across various OS environments.

### Performance Benchmarking

1. **Load Testing**
   - To identify system limitations.
   - Tools: Used Postman -- Alternatives: Apache JMeter, Artillery.io, or LoadRunner.
   - Scenarios:
     - Single user continuously requesting ZIP files.
     - Multiple users making simultaneous requests.

2. **Stress Testing**
   - To validate system stability under extreme conditions.
   - Gradually increase the load to identify the breaking point.

3. **Concurrency Testing**
   - To identify race conditions or deadlocks.
   - Tools: ThreadSanitizer, custom test scripts.

4. **Memory Profiling**
   - To detect memory leaks or excessive memory consumption.
   - Tools: Used VS Diagnostic Tools and DotMemory -- Alternatives: ANTS Memory Profiler.

5. **CPU Profiling**
   - To ensure that the application makes optimal use of CPU.
   - Tools: Used DotTrace -- Alternatives: PerfView.

### Metrics to Measure

1. **Throughput**
   - Number of requests that can be handled per unit time.

2. **Latency**
   - Time taken to complete a single request.

3. **Memory Usage**
   - Memory consumed during request processing.

4. **CPU Utilization**
   - Percentage of CPU used.

5. **Error Rates**
   - Number of failed requests versus successful requests.

### Data Analysis

- Collect and analyze the benchmarking data to identify bottlenecks or potential areas of improvement.

### Continuous Monitoring

- Use monitoring tools like Grafana, Prometheus, or Azure Monitor to continuously keep track of performance metrics.

By applying these testing and benchmarking strategies, we aim to ensure that the application not only meets its functional requirements but also performs well under various conditions. These tests will help in teasing out problems related to scalability and performance, thereby making the system more robust and reliable.

---

## Conclusion

The project demonstrates how to create ZIP files in an ASP.NET minimal API, with improvements aimed at enhancing performance, scalability, and reliability. By addressing these specific concerns, the application becomes better equipped to handle real-world workloads and edge cases.
