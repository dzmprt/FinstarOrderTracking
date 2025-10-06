using System.Linq.Expressions;
using FOT.Application.Common.Abstractions.Infrastructure.Database;
using FOT.Application.UseCase.Orders.Commands.UpdateOrderStatus;
using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;
using MitMediator;
using Moq;

namespace FOT.Application.Tests;

public class UpdateOrderStatusCommandTests
{
    [Fact]
    public async Task Handler_UpdatesOrder_ReturnsDto()
    {
        var order = new Order("desc", DateTimeOffset.UtcNow);
        var repo = new Mock<IBaseRepository<Order>>();
        repo.Setup(r => 
                r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Order,bool>>?>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        
        repo.Setup(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        
        var mediator = new Mock<IMediator>();
        var handler = new UpdateOrderStatusCommandHandler(repo.Object, mediator.Object);
        var cmd = new UpdateOrderStatusCommand { NewStatus = OrderStatus.Shipped };
        cmd.SetOrderNumber(order.OrderNumber);
        var result = await handler.HandleAsync(cmd, CancellationToken.None);
        Assert.Equal(OrderStatus.Shipped, result.Status);
    }

    [Fact]
    public async Task Handler_OrderNotFound_Throws()
    {
        var repo = new Mock<IBaseRepository<Order>>();
        repo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Order,bool>>?>(), It.IsAny<CancellationToken>())).ReturnsAsync((Order)null);
        var mediator = new Mock<IMediator>();
        var handler = new UpdateOrderStatusCommandHandler(repo.Object, mediator.Object);
        var cmd = new UpdateOrderStatusCommand { NewStatus = OrderStatus.Shipped };
        cmd.SetOrderNumber(Guid.NewGuid());
        await Assert.ThrowsAsync<FOT.Application.UseCase.Orders.OrderNotFoundException>(() => handler.HandleAsync(cmd, CancellationToken.None).AsTask());
    }

    [Fact]
    public void Validator_ValidCommand_Passes()
    {
        var validator = new UpdateOrderStatusCommandValidator();
        var cmd = new UpdateOrderStatusCommand { NewStatus = OrderStatus.Shipped };
        cmd.SetOrderNumber(Guid.NewGuid());
        var result = validator.Validate(cmd);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void Validator_InvalidOrderNumber_Fails(string guidStr)
    {
        var validator = new UpdateOrderStatusCommandValidator();
        var cmd = new UpdateOrderStatusCommand { NewStatus = OrderStatus.Shipped };
        cmd.SetOrderNumber(Guid.Parse(guidStr));
        var result = validator.Validate(cmd);
        Assert.False(result.IsValid);
    }
}