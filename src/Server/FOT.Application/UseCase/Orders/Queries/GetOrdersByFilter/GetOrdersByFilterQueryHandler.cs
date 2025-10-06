using System.Linq.Expressions;
using FOT.Application.Common;
using FOT.Application.Common.Abstractions.Infrastructure.Database;
using FOT.Domain.Orders;
using MitMediator;

namespace FOT.Application.UseCase.Orders.Queries.GetOrdersByFilter;

/// <summary>
/// Handler for <see cref="GetOrdersByFilterQuery"/>.
/// </summary>
/// <param name="ordersProvider">Orders provider.</param>
public class GetOrdersByFilterQueryHandler(IBaseProvider<Order> ordersProvider)
    : IRequestHandler<GetOrdersByFilterQuery, ListResult<OrderDto>>
{
    /// <inheritdoc/>
    public async ValueTask<ListResult<OrderDto>> HandleAsync(GetOrdersByFilterQuery request,
        CancellationToken cancellationToken)
    {
        Expression<Func<Order, bool>> filter = o =>
            (request.OrderStatuses == null || request.OrderStatuses.Length == 0 ||
             request.OrderStatuses.Contains(o.Status)) &&
            (request.CreatedFrom == null || o.CreatedAt >= request.CreatedFrom.Value.DateTime) &&
            (request.CreatedTo == null || o.CreatedAt <= request.CreatedTo.Value.DateTime) &&
            (request.UpdatedFrom == null || o.UpdatedAt >= request.UpdatedFrom.Value.DateTime) &&
            (request.UpdatedTo == null || o.UpdatedAt <= request.UpdatedTo.Value.DateTime) &&
            (request.FreeText == null ||
             request.FreeText.Length == 0 ||
             o.Description.Contains(request.FreeText)
             ||
             o.OrderNumber.ToString().Contains(request.FreeText)
            );

        Expression<Func<Order, object>> orderSelector = request.OrderBy switch
        {
            GetOrdersOrderByEnum.ByNumber => o => o.OrderNumber,
            GetOrdersOrderByEnum.ByStatus => o => o.Status,
            GetOrdersOrderByEnum.ByCreatedDateTime => o => o.CreatedAt,
            GetOrdersOrderByEnum.ByUpdatedDateTime => o => o.UpdatedAt.Value,
            _ => o => o.CreatedAt
        };

        var orders = await ordersProvider
            .SearchAsync(
                filter,
                orderSelector,
                request.Limit,
                request.Offset
                , cancellationToken);

        var totalCount = await ordersProvider.CountAsync(filter, cancellationToken);
        return new ListResult<OrderDto>(orders.Select(o => new OrderDto(o)).ToArray(), totalCount);
    }
}
