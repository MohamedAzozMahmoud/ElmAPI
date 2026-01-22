// =====================================================
// PerformanceBehavior.cs - Slow Request Detection
// =====================================================
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Elm.Application.Behaviors;

public sealed class PerformanceOptions
{
    public int SlowRequestThresholdMs { get; set; } = 500;
}

public sealed class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly PerformanceOptions _options;

    public PerformanceBehavior(
        ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
        IOptions<PerformanceOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > _options.SlowRequestThresholdMs)
        {
            _logger.LogWarning(
                "⚠️ Slow Request Detected: {RequestName} took {ElapsedMs}ms (Threshold: {Threshold}ms)",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds,
                _options.SlowRequestThresholdMs);
        }

        return response;
    }
}