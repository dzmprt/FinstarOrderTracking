using FOT.Application.Abstractions;
using Microsoft.Extensions.Logging;
using MitMediator;

namespace FOT.Application.Common.Notifications;

/// <summary>
/// Domain event outbox notification handler.
/// </summary>
/// <param name="outboxRepository">Outbox repository.</param>
public class DomainEventOutboxNotificationHandler(
    IBaseRepository<DomainEventOutbox> outboxRepository,
    ILogger<DomainEventOutboxNotificationHandler> logger) : INotificationHandler<DomainEventOutbox>
{
    /// <inheritdoc />
    public async ValueTask HandleAsync(DomainEventOutbox notification, CancellationToken cancellationToken)
    {
        await outboxRepository.AddAsync(notification, cancellationToken);
        logger.LogInformation("Saved {DomainEventOutboxNotification} to database: {@notification}", nameof(DomainEventOutbox), notification);
    }
}