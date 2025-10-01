using FOT.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FOT.DatabaseInfrastructure.Configurations;

/// <summary>
/// Configuration for <see cref="Order"/>.
/// </summary>
public class OrderTypeConfiguration : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.OrderNumber);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(Order.MaxDescriptionLength);
    }
}