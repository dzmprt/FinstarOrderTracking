using FOT.Application.Common;
using FOT.Application.UseCase.Orders;
using FOT.Application.UseCase.Orders.Commands.CreateOrder;
using FOT.Application.UseCase.Orders.Commands.UpdateOrderStatus;
using FOT.Application.UseCase.Orders.Queries.GetOrder;
using FOT.Application.UseCase.Orders.Queries.GetOrdersByFilter;
using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;
using Microsoft.AspNetCore.Mvc;
using MitMediator;

namespace FOT.WebApi.Endpoints;

/// <summary>
/// Orders API endpoints.
/// </summary>
internal static class OrdersApi
{
    private const string Tag = "orders";

    private const string ApiUrl = "api";

    private const string Version = "v1";

    /// <summary>
    /// Use Orders APIendpoints.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/>.</param>
    /// <returns><see cref="WebApplication"/>.</returns>
    public static WebApplication UseOrdersApi(this WebApplication app)
    {
        #region Queries

        app.MapGet($"{ApiUrl}/{Version}/{Tag}/{{orderNumber}}", GetOrderByIdAsync)
            .WithTags(Tag)
            .WithDescription("Get order by id.")
            .WithGroupName(Version)
            .Produces<OrderDto>();

        app.MapGet($"{ApiUrl}/{Version}/{Tag}", GetOrdersByFilterAsync)
            .WithTags(Tag)
            .WithDescription("Get orders by filter.")
            .WithGroupName(Version)
            .Produces<OrderDto[]>();

        #endregion

        #region Commands

        app.MapPost($"{ApiUrl}/{Version}/{Tag}", CreateOrderAsync)
            .WithTags(Tag)
            .WithDescription("Create order.")
            .WithGroupName(Version)
            .Produces<OrderDto>();

        app.MapPatch($"{ApiUrl}/{Version}/{Tag}/{{orderNumber}}/status", UpdateOrderStatusAsync)
            .WithTags(Tag)
            .WithDescription("Update order.")
            .WithGroupName(Version)
            .Produces<OrderDto>();

        #endregion

        return app;
    }

    private static ValueTask<OrderDto> GetOrderByIdAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid orderNumber,
        CancellationToken cancellationToken)
    {
        return mediator.SendAsync<GetOrderQuery, OrderDto>(new GetOrderQuery
        {
            OrderNumber = orderNumber,
        }, cancellationToken);
    }

    private static async ValueTask<IResult> GetOrdersByFilterAsync(
        [FromServices] IMediator mediator,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromQuery] int limit,
        [FromQuery] int? offset,
        [FromQuery] string? freeText,
        [FromQuery] OrderStatusEnum[]? orderStatus,
        [FromQuery] DateTimeOffset? createdFrom,
        [FromQuery] DateTimeOffset? createdTo,
        [FromQuery] DateTimeOffset? updatedFrom,
        [FromQuery] DateTimeOffset? updatedTo,
        [FromQuery] GetOrdersOrderByEnum? orderBy,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync<GetOrdersByFilterQuery, ListResult<OrderDto>>(new GetOrdersByFilterQuery
        {
            Limit = limit,
            Offset = offset,
            FreeText = freeText,
            OrderStatuses = orderStatus,
            CreatedFrom = createdFrom,
            CreatedTo = createdTo,
            UpdatedFrom = updatedFrom,
            UpdatedTo = updatedTo,
            OrderBy = orderBy
        }, cancellationToken);
        
        httpContextAccessor.HttpContext!.Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());

        return Results.Ok(result.Items);
    }

    private static ValueTask<OrderDto> CreateOrderAsync(
        [FromServices] IMediator mediator,
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        return mediator.SendAsync<CreateOrderCommand, OrderDto>(command, cancellationToken);
    }

    private static ValueTask<OrderDto> UpdateOrderStatusAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid orderNumber,
        [FromBody] UpdateOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        command.SetOrderNumber(orderNumber);
        return mediator.SendAsync<UpdateOrderStatusCommand, OrderDto>(command, cancellationToken);
    }
}