using FOT.Application.Common.Abstractions.Infrastructure.Database;
using FOT.Application.UseCase.Orders.Queries.GetOrdersByFilter;
using FOT.Domain.Orders;
using Moq;

namespace FOT.Application.Tests;

public class GetOrdersByFilterQueryTests
{
    [Fact]
    public async Task Handler_ReturnsFilteredOrders()
    {
        var orders = new Order[2]
        {
            new Order("desc1", DateTimeOffset.UtcNow),
            new Order("desc2", DateTimeOffset.UtcNow)
        };
        var provider = new Mock<IBaseProvider<Order>>();
        provider.Setup(p => p.SearchAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Order, bool>>>(), It.IsAny<System.Linq.Expressions.Expression<Func<Order, object>>>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(orders);
        provider.Setup(p => p.CountAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(orders.Count);
        var handler = new GetOrdersByFilterQueryHandler(provider.Object);
        var query = new GetOrdersByFilterQuery { Limit = 10 };
        var result = await handler.HandleAsync(query, CancellationToken.None);
        Assert.Equal(orders.Length, result.Items.Length);
        Assert.Equal(orders.Length, result.TotalCount);
    }

    [Fact]
    public void Validator_ValidQuery_Passes()
    {
        var validator = new GetOrdersByFilterQueryValidator();
        var query = new GetOrdersByFilterQuery { Limit = 10, Offset = 0 };
        var result = validator.Validate(query);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_InvalidLimit_Fails()
    {
        var validator = new GetOrdersByFilterQueryValidator();
        var query = new GetOrdersByFilterQuery { Limit = 0 };
        var result = validator.Validate(query);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_InvalidOffset_Fails()
    {
        var validator = new GetOrdersByFilterQueryValidator();
        var query = new GetOrdersByFilterQuery { Limit = 10, Offset = -1 };
        var result = validator.Validate(query);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_InvalidFreeText_Fails()
    {
        var validator = new GetOrdersByFilterQueryValidator();
        var query = new GetOrdersByFilterQuery { Limit = 10, FreeText = new string('a', 201) };
        var result = validator.Validate(query);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_InvalidDateRange_Fails()
    {
        var validator = new GetOrdersByFilterQueryValidator();
        var query = new GetOrdersByFilterQuery { Limit = 10, CreatedFrom = DateTimeOffset.UtcNow, CreatedTo = DateTimeOffset.UtcNow.AddDays(-1) };
        var result = validator.Validate(query);
        Assert.False(result.IsValid);
    }
}