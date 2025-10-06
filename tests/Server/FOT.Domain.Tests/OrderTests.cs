using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;

namespace FOT.Domain.Tests;

public class OrderTests
{
    [Fact]
    public void Constructor_ValidDescription_SetsProperties()
    {
        var now = DateTimeOffset.UtcNow;
        var order = new Order("Test order", now);
        Assert.Equal("Test order", order.Description);
        Assert.Equal(OrderStatus.Created, order.Status);
        Assert.Equal(now.UtcDateTime, order.CreatedAt);
        Assert.Null(order.UpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_InvalidDescription_Throws(string desc)
    {
        var now = DateTimeOffset.UtcNow;
        Assert.ThrowsAny<ArgumentException>(() => new Order(desc, now));
    }

    [Fact]
    public void Constructor_DescriptionTooShort_Throws()
    {
        var now = DateTimeOffset.UtcNow;
        Assert.ThrowsAny<ArgumentException>(() => new Order("", now));
    }

    [Fact]
    public void Constructor_DescriptionTooLong_Throws()
    {
        var now = DateTimeOffset.UtcNow;
        var longDesc = new string('a', Order.MaxDescriptionLength + 1);
        Assert.ThrowsAny<ArgumentException>(() => new Order(longDesc, now));
    }

    [Fact]
    public void ChangeStatus_ValidTransition_UpdatesStatusAndUpdatedAt()
    {
        var now = DateTimeOffset.UtcNow;
        var order = new Order("Test", now);
        order.ChangeStatus(OrderStatus.Shipped, now.AddMinutes(1));
        Assert.Equal(OrderStatus.Shipped, order.Status);
        Assert.Equal(now.AddMinutes(1).UtcDateTime, order.UpdatedAt);
    }

    [Fact]
    public void ChangeStatus_InvalidTransition_Throws()
    {
        var now = DateTimeOffset.UtcNow;
        var order = new Order("Test", now);
        Assert.Throws<InvalidOperationException>(() => order.ChangeStatus(OrderStatus.Delivered, now.AddMinutes(1)));
    }

    [Fact]
    public void ChangeStatus_SameStatus_DoesNothing()
    {
        var now = DateTimeOffset.UtcNow;
        var order = new Order("Test", now);
        order.ChangeStatus(OrderStatus.Created, now.AddMinutes(1));
        Assert.Equal(OrderStatus.Created, order.Status);
        Assert.Null(order.UpdatedAt);
    }
}