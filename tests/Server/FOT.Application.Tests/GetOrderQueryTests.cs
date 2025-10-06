using System.Linq.Expressions;
using FOT.Application.Common.Abstractions.Infrastructure.Database;
using FOT.Application.UseCase.Orders.Queries.GetOrder;
using FOT.Domain.Orders;
using Moq;

namespace FOT.Application.Tests;

public class GetOrderQueryTests
{
    [Fact]
    public async Task Handler_ReturnsOrderDto()
    {
        var order = new Order("desc", DateTimeOffset.UtcNow);
        var provider = new Mock<IBaseProvider<Order>>();
        provider.Setup(p => p.FirstOrDefaultAsync(It.IsAny<Expression<Func<Order, bool>>?>(), It.IsAny<CancellationToken>())).ReturnsAsync(order);
        var handler = new GetOrderQueryHandler(provider.Object);
        var query = new GetOrderQuery { OrderNumber = order.OrderNumber };
        var result = await handler.HandleAsync(query, CancellationToken.None);
        Assert.Equal(order.OrderNumber, result.OrderNumber);
    }

    [Fact]
    public async Task Handler_OrderNotFound_Throws()
    {
        var provider = new Mock<IBaseProvider<Order>>();
        provider.Setup(p => p.FirstOrDefaultAsync(It.IsAny<Expression<Func<Order, bool>>?>(), It.IsAny<CancellationToken>())).ReturnsAsync((Order)null);
        var handler = new GetOrderQueryHandler(provider.Object);
        var query = new GetOrderQuery { OrderNumber = Guid.NewGuid() };
        await Assert.ThrowsAsync<FOT.Application.UseCase.Orders.OrderNotFoundException>(() => handler.HandleAsync(query, CancellationToken.None).AsTask());
    }

    [Fact]
    public void Validator_ValidQuery_Passes()
    {
        var validator = new GetOrderQueryValidator();
        var query = new GetOrderQuery { OrderNumber = Guid.NewGuid() };
        var result = validator.Validate(query);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_InvalidOrderNumber_Fails()
    {
        var validator = new GetOrderQueryValidator();
        var query = new GetOrderQuery { OrderNumber = Guid.Empty };
        var result = validator.Validate(query);
        Assert.False(result.IsValid);
    }
}