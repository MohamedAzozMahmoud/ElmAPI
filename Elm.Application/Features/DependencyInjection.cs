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
            cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODAwNDg5NjAwIiwiaWF0IjoiMTc2OTAxOTUzMCIsImFjY291bnRfaWQiOiIwMTliZTFjNWNiMmE3ZDQyYjNmOWY2ZTEwMTdkNWNhNyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2Znd2ZxZGg3bjJjbjRoMjcxbnFtamhwIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.afR2Hqb4xeW1Ja-e6neE_8bkcyUPJ6UKfLrx6zg7TRcb799fuVyzWpNGW2hrqXFgzjreQ4un-onQ_WrYItGVqvSuZoDCyrbgLSxXWwec9YFbUdAvugj1N33Q-IUAj4lbP_E1PORumGqUq9hUmpJyseuJdp9JUaYW5TcIeh_Iu5Hke73whCNFoH-df1boeuC48HC29WPCchBMS_5ImLNBmSRfYVNdz-25flfUR0S2hjzBRR_wDE-mgqqTd4RhLW7QypvUjV6nygj5IYVLaiYewvgdtGrhkNU-QBb-RlpeKwxVVtZ70VnpvClbtNEliprZccbH8yUbkqETNQTSLfAsDg";
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