using MitMediator;
using Microsoft.Extensions.Logging;

namespace FOT.Application.Common.Behaviors;

/// <summary>
/// Logging behavior.
/// </summary>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc/>
    public async ValueTask<TResponse> HandleAsync(TRequest request, IRequestHandlerNext<TRequest, TResponse> nextPipe, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        logger.LogInformation("Handling {RequestName}: {@Request}", requestName, request);
        
        try
        {
            var response = await nextPipe.InvokeAsync(request, cancellationToken);
            logger.LogInformation("Handled {RequestName} successfully", requestName);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling {RequestName}: {@Request}", requestName, request);
            throw;
        }
    }
}