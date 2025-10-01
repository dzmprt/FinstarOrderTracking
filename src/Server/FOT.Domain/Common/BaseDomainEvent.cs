namespace FOT.Domain.Common;

/// <summary>
/// Base event model.
/// </summary>
public abstract class BaseDomainEvent
{
    /// <summary>
    /// Event created date time.
    /// </summary>
    public DateTimeOffset EventCreatedAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseDomainEvent"/>.
    /// </summary>
    /// <param name="eventCreatedAt">Event created date time.</param>
    public BaseDomainEvent(DateTimeOffset eventCreatedAt)
    {
        EventCreatedAt = eventCreatedAt;
    }
};