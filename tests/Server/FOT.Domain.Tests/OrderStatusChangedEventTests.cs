using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;
using FOT.Domain.Orders.Events;

namespace FOT.Domain.Tests;

public class OrderStatusChangedEventTests
{
    [Fact]
    public void Constructor_SetsPropertiesFromOrder()
    {
        var now = DateTimeOffset.UtcNow;
        var order = new Order("desc", now);
        order.ChangeStatus(OrderStatus.Shipped, now.AddMinutes(1));
        var evt = new OrderStatusChangedEvent(order);
        Assert.Equal(order.OrderNumber, evt.OrderNumber);
        Assert.Equal(order.Status, evt.NewStatus);
        Assert.Equal(order.UpdatedAt, evt.UpdatedAt);
        Assert.True(evt.EventCreatedAt <= DateTimeOffset.UtcNow);
    }
}