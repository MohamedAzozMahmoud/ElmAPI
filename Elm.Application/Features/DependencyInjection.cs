// =====================================================
// DependencyInjection.cs
// =====================================================
using Elm.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Elm.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // MediatR

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        // FluentValidation - Auto-register all validators
        services.AddValidatorsFromAssembly(assembly);

        // ✅ Pipeline Behaviors - ORDER MATTERS!
        // 1. Logging (outermost - logs everything)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // 2. Performance monitoring
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

        // 3. Validation (before transaction)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // 4. Transaction (innermost - wraps the actual handler)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        // Performance options
        services.Configure<PerformanceOptions>(options =>
        {
            options.SlowRequestThresholdMs = 500;
        });

        return services;
    }
}