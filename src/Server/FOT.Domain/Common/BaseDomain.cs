namespace FOT.Domain.Common;

/// <summary>
/// Base domain model.
/// </summary>
public class BaseDomain
{
    private readonly List<BaseDomainEvent> _domainEvents = new();

    /// <summary>
    /// Domain events.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyCollection<BaseDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    /// <summary>
    /// Add domain event.
    /// </summary>
    /// <param name="domainEvent"></param>
    public void AddDomainEvent(BaseDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Remove domain events.
    /// </summary>
    /// <param name="domainEvent"></param>
    public void RemoveDomainEvent(BaseDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Clear domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
