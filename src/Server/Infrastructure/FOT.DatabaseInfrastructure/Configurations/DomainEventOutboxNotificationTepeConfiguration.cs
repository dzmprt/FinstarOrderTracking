using FOT.Application.Common.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FOT.DatabaseInfrastructure.Configurations;

/// <summary>
/// Configuration for <see cref="DomainEventOutbox"/>.
/// </summary>
public class DomainEventOutboxNotificationTepeConfiguration : IEntityTypeConfiguration<DomainEventOutbox>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<DomainEventOutbox> builder)
    {
        builder.HasKey(e => e.DomainEventOutboxId);
        builder.Property(e => e.AggregateType).HasMaxLength(1000);
        builder.Property(e => e.AggregateId).HasMaxLength(1000);
        builder.Property(e => e.EventCode).HasMaxLength(1000);
        builder.Property(e => e.Payload).HasMaxLength(10_000);
    }
}