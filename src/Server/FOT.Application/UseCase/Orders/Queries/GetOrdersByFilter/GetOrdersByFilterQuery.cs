using FOT.Application.Common;
using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;
using MitMediator;

namespace FOT.Application.UseCase.Orders.Queries.GetOrdersByFilter;

/// <summary>
/// Get orders by filter query.
/// </summary>
public class GetOrdersByFilterQuery : IRequest<ListResult<OrderDto>>
{
     /// <summary>
     /// Order statuses.
     /// </summary>
     public OrderStatusEnum[]? OrderStatuses { get; init; }
     
     /// <summary>
     /// Created date from.
     /// </summary>
     public DateTimeOffset? CreatedFrom { get; init; }
     
     /// <summary>
     /// Created date to.
     /// </summary>
     public DateTimeOffset? CreatedTo { get; init; }
     
     /// <summary>
     /// Updated date from.
     /// </summary>
     public DateTimeOffset? UpdatedFrom { get; init; }
     
     /// <summary>
     /// Updated date to.
     /// </summary>
     public DateTimeOffset? UpdatedTo { get; init; }
     
     /// <summary>
     /// Limit.
     /// </summary>
     public int Limit { get; init; }
    
     /// <summary>
     /// Offset.
     /// </summary>
     public int? Offset { get; init; }
     
     /// <summary>
     /// Free text.
     /// </summary>
     public string? FreeText { get; init; }
     
     /// <summary>
     /// Order by.
     /// </summary>
     public GetOrdersOrderByEnum? OrderBy { get; init; }
}