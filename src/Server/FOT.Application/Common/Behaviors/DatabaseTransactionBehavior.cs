using FOT.Application.Common.Abstractions.Infrastructure.Database;
using MitMediator;

namespace FOT.Application.Common.Behaviors;

/// <summary>
/// Database transaction pipe.
/// </summary>
public class DatabaseTransactionBehavior<TRequest, TResponse>(IDatabaseTransactionCreator databaseTransactionCreator)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc/>
    public async ValueTask<TResponse> HandleAsync(TRequest request, IRequestHandlerNext<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = await databaseTransactionCreator.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await next.InvokeAsync(request, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}