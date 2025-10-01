using FOT.Application.Abstractions;
using FOT.Domain.Orders;
using MitMediator;

namespace FOT.Application.UseCase.Orders.Commands.CreateOrder;

/// <summary>
/// Handler for <see cref="CreateOrderCommand"/>.
/// </summary>
/// <param name="ordersRepository">Orders repository.</param>
public class CreateOrderCommandHandler(IBaseRepository<Order> ordersRepository) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    /// <inheritdoc/>
    public async ValueTask<OrderDto> HandleAsync(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(request.Description, DateTimeOffset.UtcNow);
        await ordersRepository.AddAsync(order, cancellationToken);
        return new OrderDto(order);
    }
}