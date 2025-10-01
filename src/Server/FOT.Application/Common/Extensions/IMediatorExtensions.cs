using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using FOT.Application.Common.Notifications;
using FOT.Domain.Common;
using MitMediator;

namespace FOT.Application.Common.Extensions;

/// <summary>
/// Extension for IMediator.
/// </summary>
public static class MediatorExtensions
{
    /// <summary>
    /// Publish domain events as <see cref="DomainEventOutbox"/>.
    /// </summary>
    public static async ValueTask PublishDomainEvents<TDomain>(
        this IMediator mediator, 
        TDomain domainModel, 
        string domainModelId,
        CancellationToken cancellationToken) where TDomain: BaseDomain
    {
        if (domainModel.GetDomainEvents().Any())
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            foreach (var orderDomainEvent in domainModel.GetDomainEvents())
            {
    
                await mediator.PublishAsync(new DomainEventOutbox
                {
                    AggregateType = domainModel.GetType().Name,
                    AggregateId = domainModelId,
                    EventCode = orderDomainEvent.GetType().Name,
                    OccurredAt = orderDomainEvent.EventCreatedAt.UtcDateTime,
                    Payload = JsonSerializer.Serialize((object)orderDomainEvent, options)
                }, cancellationToken);
            }
        }
    }
}