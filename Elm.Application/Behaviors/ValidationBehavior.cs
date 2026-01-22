using Elm.Application.Contracts;
using Elm.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Elm.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    // ✅ Static cache for compiled failure factory delegates
    private static readonly ConcurrentDictionary<Type, Func<List<ValidationError>, int, object>>
        FailureFactoryCache = new();

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        LogValidationFailures(failures);

        var validationErrors = failures.Select(MapToValidationError).ToList();

        return CreateFailureResponse(validationErrors);
    }

    private void LogValidationFailures(List<ValidationFailure> failures)
    {
        _logger.LogWarning(
            "Validation failed for {RequestName}. Errors: {Errors}",
            typeof(TRequest).Name,
            string.Join("; ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}")));
    }

    private static ValidationError MapToValidationError(ValidationFailure failure) => new()
    {
        PropertyName = failure.PropertyName,
        ErrorMessage = failure.ErrorMessage,
        ErrorCode = failure.ErrorCode ?? "VALIDATION_ERROR",
        AttemptedValue = failure.AttemptedValue,
        Severity = failure.Severity switch
        {
            FluentValidation.Severity.Warning => Contracts.Severity.Warning,
            FluentValidation.Severity.Info => Contracts.Severity.Info,
            _ => Contracts.Severity.Error
        }
    };

    private static TResponse CreateFailureResponse(List<ValidationError> errors)
    {
        var responseType = typeof(TResponse);

        // ✅ Non-generic Result
        if (responseType == typeof(Result))
            return (TResponse)(object)Result.Failure(errors);

        // ✅ Generic Result<T> with cached compiled delegate
        if (responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var factory = FailureFactoryCache.GetOrAdd(
                responseType,
                CreateFailureFactory);

            return (TResponse)factory(errors, 400);
        }

        throw new AppValidationException(errors);
    }

    /// <summary>
    /// Creates a compiled delegate for Result<T>. Failure method
    /// This is called only once per type and cached
    /// </summary>
    private static Func<List<ValidationError>, int, object> CreateFailureFactory(Type resultType)
    {
        var dataType = resultType.GetGenericArguments()[0];
        var concreteResultType = typeof(Result<>).MakeGenericType(dataType);

        var failureMethod = concreteResultType.GetMethod(
            nameof(Result<object>.Failure),
            [typeof(List<ValidationError>), typeof(int)]);

        if (failureMethod is null)
            throw new InvalidOperationException(
                $"Failure method not found on {concreteResultType.Name}");

        // Build:  (errors, statusCode) => Result<T>.Failure(errors, statusCode)
        var errorsParam = Expression.Parameter(typeof(List<ValidationError>), "errors");
        var statusCodeParam = Expression.Parameter(typeof(int), "statusCode");

        var callExpression = Expression.Call(failureMethod, errorsParam, statusCodeParam);
        var castExpression = Expression.Convert(callExpression, typeof(object));

        var lambda = Expression.Lambda<Func<List<ValidationError>, int, object>>(
            castExpression,
            errorsParam,
            statusCodeParam);

        return lambda.Compile();
    }
}