// =================================================
// TransactionBehavior. cs - الإصدار المُصحَّح
// =================================================
using Elm.Application.Contracts;
using Elm.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elm.Application.Behaviors;
public interface ITransactionalCommand
{
    // يمكنك إضافة خصائص أو طرق هنا إذا لزم الأمر

}
public sealed class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // ✅ استخدم AppDbContext بدلاً من DbContext
    private readonly AppDbContext _context;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        AppDbContext context,  // ✅ غيّرها من DbContext إلى AppDbContext
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Only apply to transactional commands
        if (request is not ITransactionalCommand)
            return await next();

        var requestName = typeof(TRequest).Name;

        if (_context.Database.CurrentTransaction is not null)
            return await next();

        await using var transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogDebug("Beginning transaction for {RequestName}", requestName);

            var response = await next();

            if (response is IResult { IsSuccess: true })
            {
                await transaction.CommitAsync(cancellationToken);
                _logger.LogDebug("Transaction committed for {RequestName}", requestName);
            }
            else
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogDebug("Transaction rolled back for {RequestName}", requestName);
            }

            return response;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Transaction rolled back for {RequestName}", requestName);
            throw;
        }
    }
}