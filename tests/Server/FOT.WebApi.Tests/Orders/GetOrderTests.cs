using System.Net;
using FOT.Application.Common.Abstractions.Infrastructure.Database;
using FOT.Domain.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace FOT.WebApi.Tests.Orders;

public class GetOrderTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task Get_MustReturnOrder_WhenOrderExists()
    {
        // Arrange: create an order first
        var client = factory.CreateClient();
        var repository = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IBaseRepository<Order>>();
        var createdOrder = await repository.AddAsync(new Order("Test order", DateTime.UtcNow), CancellationToken.None);

        // Act
        var response = await client.GetAsync($"api/v1/orders/{createdOrder.OrderNumber}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains(createdOrder.OrderNumber.ToString(), result);
    }

    [Fact]
    public async Task Get_MustReturnNotFound_WhenOrderDoesNotExist()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync($"api/v1/orders/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}