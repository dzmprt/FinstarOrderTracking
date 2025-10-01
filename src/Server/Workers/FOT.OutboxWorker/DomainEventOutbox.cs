namespace FOT.OutboxWorker;

/// <summary>
/// Domain event notification for MQ.
/// </summary>
public class DomainEventOutbox 
{
    public int DomainEventOutboxId { get; init; }
    
    public string AggregateType { get; init; }
    
    public string AggregateId { get; init; }
    
    public string EventCode { get; init; }
    
    public string Payload { get; init; }
    
    public DateTime OccurredAt { get; init; }
    
    public DateTime? ProcessedAt { get; init; }
}