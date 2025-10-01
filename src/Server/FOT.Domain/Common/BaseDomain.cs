namespace FOT.Domain.Common;

/// <summary>
/// Base domain model.
/// </summary>
public class BaseDomain
{
    private readonly List<BaseDomainEvent> _domainEvents = new();
    
    public IReadOnlyCollection<BaseDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}