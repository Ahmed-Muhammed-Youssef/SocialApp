using API.Benchmark.Helpers;
using Application.Common.Interfaces;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace API.Benchmark.Benchmarks;

/// <summary>
/// Shared plumbing for benchmarks that run against a real SQL Server through the real DI container.
/// </summary>
/// <remarks>
/// Every iteration gets a fresh scope, mirroring one HTTP request: a new <c>DbContext</c> with an empty
/// change tracker and fresh repository instances. Scope creation and cache clearing happen in
/// <see cref="IterationSetup"/> — outside the measured region, which is the mistake the previous version
/// of these benchmarks made by building its dependencies inside the <c>[Benchmark]</c> method.
/// </remarks>
public abstract class DatabaseBenchmarkBase
{
    private ServiceProvider? _provider;
    private IServiceScope? _scope;

    protected IUnitOfWork UnitOfWork { get; private set; } = null!;

    protected ICurrentUserService CurrentUser { get; private set; } = null!;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _provider = BenchmarkHost.CreateServiceProvider();

        await BenchmarkHost.VerifyDatabaseAsync(_provider);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        BenchmarkHost.ClearCache(_provider!);

        _scope = _provider!.CreateScope();
        UnitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        CurrentUser = _scope.ServiceProvider.GetRequiredService<ICurrentUserService>();

        OnIterationSetup();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        OnIterationCleanup();

        _scope?.Dispose();
        _scope = null;
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _provider?.Dispose();
        _provider = null;
    }

    /// <summary>Runs after the scope exists and before the measurement starts.</summary>
    protected virtual void OnIterationSetup()
    {
    }

    /// <summary>Runs after the measurement finishes and before the scope is disposed.</summary>
    protected virtual void OnIterationCleanup()
    {
    }
}
