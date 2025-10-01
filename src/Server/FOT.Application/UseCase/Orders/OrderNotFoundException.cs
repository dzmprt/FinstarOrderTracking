using FOT.Application.Common.Exceptions;
using FOT.Domain.Orders;

namespace FOT.Application.UseCase.Orders;

public class OrderNotFoundException(Guid orderNumber)
    : NotFoundException($"Not found {nameof(Order)} with {nameof(Order.OrderNumber)} = {orderNumber}");