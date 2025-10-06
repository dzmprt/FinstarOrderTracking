using System.Net;
using System.Net.Http.Json;
using FOT.Application.Common.Abstractions.Infrastructure.Database;
using FOT.Domain.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace FOT.WebApi.Tests.Orders;

public class UpdateOrderStatusTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task Patch_MustUpdateOrderStatus_WhenOrderExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var repository = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IBaseRepository<Order>>();
        var orderToUpdate =
            await repository.AddAsync(new Order("Order to update", DateTime.UtcNow), CancellationToken.None);

        // Act
        var patchResponse = await client.PatchAsJsonAsync($"api/v1/orders/{orderToUpdate.OrderNumber}/status", new { NewStatus = 2 });
        
        //Assert
        patchResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);
        var updatedOrder = await patchResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        int status = updatedOrder.TryGetProperty("status", out var statusProp) ? statusProp.GetInt32() : -1;
        Assert.Equal(2, status);
    }

    [Fact]
    public async Task Patch_MustReturnNotFound_WhenOrderDoesNotExist()
    {
        var client = factory.CreateClient();
        var patchResponse = await client.PatchAsJsonAsync($"api/v1/orders/{Guid.NewGuid()}/status", new { NewStatus = 1 });
        Assert.Equal(HttpStatusCode.NotFound, patchResponse.StatusCode);
    }
    
    [Fact]
    public async Task Post_MustReturnConflict_WhenReplayNonceError()
    {
        // Arrange
        var client = factory.CreateClient();
        var replayNonce = Guid.NewGuid().ToString();
        client.DefaultRequestHeaders.Add("Replay-Nonce", replayNonce);
        var repository = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IBaseRepository<Order>>();
        var orderToUpdate =
            await repository.AddAsync(new Order("Order to update", DateTime.UtcNow), CancellationToken.None);

        // Act
        await client.PatchAsJsonAsync($"api/v1/orders/{orderToUpdate.OrderNumber}/status", new { NewStatus = 2 });
        var response = await client.PatchAsJsonAsync($"api/v1/orders/{orderToUpdate.OrderNumber}/status", new { NewStatus = 3 });
        
        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}