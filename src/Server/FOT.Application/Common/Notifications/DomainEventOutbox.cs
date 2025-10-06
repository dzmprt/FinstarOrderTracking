using MitMediator;

namespace FOT.Application.Common.Notifications;

/// <summary>
/// Domain event notification for MQ.
/// </summary>
public class DomainEventOutbox : INotification
{
    /// <summary>
    /// Domain event outbox id.
    /// </summary>
    public int DomainEventOutboxId { get; private set; }

    /// <summary>
    /// Aggregate type.
    /// </summary>
    public string AggregateType { get; init; }

    /// <summary>
    /// Aggregate id.
    /// </summary>
    public string AggregateId { get; init; }

    /// <summary>
    /// Event code.
    /// </summary>
    public string EventCode { get; init; }

    /// <summary>
    /// Payload.
    /// </summary>
    public string Payload { get; init; }

    public DateTime OccurredAt { get; init; }

    public DateTime? ProcessedAt { get; init; }
}
