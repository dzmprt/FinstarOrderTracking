using FOT.Application.Abstractions;
using FOT.Application.Common.Exceptions;
using FOT.Application.Common.Extensions;
using FOT.Domain.Orders;
using MitMediator;

namespace FOT.Application.UseCase.Orders.Commands.UpdateOrderStatus;

/// <summary>
/// Handler for <see cref="UpdateOrderStatusCommand"/>.
/// </summary>
/// <param name="ordersRepository">Orders repository.</param>
/// <param name="mediator"><see cref="IMediator"/></param>
public class UpdateOrderStatusCommandHandler(IBaseRepository<Order> ordersRepository, IMediator mediator) : IRequestHandler<UpdateOrderStatusCommand, OrderDto>
{
    /// <inheritdoc/>
    public async ValueTask<OrderDto> HandleAsync(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await ordersRepository.FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);
        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderNumber);
        }
        order.ChangeStatus(request.NewStatus, DateTimeOffset.UtcNow);
        await ordersRepository.UpdateAsync(order, cancellationToken);
        await mediator.PublishDomainEvents(order, order.OrderNumber.ToString(), cancellationToken);
        return new OrderDto(order);
    }
}