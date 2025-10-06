using System.Net;
using System.Net.Http.Json;

namespace FOT.WebApi.Tests.Orders;

public class GetOrdersByFilterTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task Get_MustReturnOrders_WhenOrdersExist()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/orders?limit=10");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var orders = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement[]>();
        Assert.NotNull(orders);
        Assert.Equal(10, orders.Length);
        Assert.True(response.Headers.Contains("X-Total-Count"));
        var totalCount = response.Headers.GetValues("X-Total-Count");
        Assert.True(int.Parse(totalCount.Single()) > 0);
    }

    [Fact]
    public async Task Get_MustReturnFilteredOrders_ByStatus()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/orders?limit=10&orderStatus=2");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement[]>();
        Assert.All(orders!, o => Assert.Equal(2, o.GetProperty("status").GetInt32()));
    }

    [Fact]
    public async Task Get_MustReturnSortedOrders_ByCreatedDateTime()
    {
        // Arrange
        var client = factory.CreateClient();
        await client.PostAsJsonAsync("api/v1/orders", new { Description = "A" });
        await client.PostAsJsonAsync("api/v1/orders", new { Description = "B" });
        await client.PostAsJsonAsync("api/v1/orders", new { Description = "C" });

        // Act
        // 3 = ByCreatedDateTime
        var response = await client.GetAsync("api/v1/orders?limit=3&orderBy=3");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement[]>();
        Assert.Equal(3, orders!.Length);
        var createdDates = orders.Select(o => o.GetProperty("createdAt").GetDateTime()).ToArray();
        Assert.True(createdDates[2] <= createdDates[1] && createdDates[1] <= createdDates[0]);
    }

    [Fact]
    public async Task Get_MustReturnPaginatedOrders()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/orders?limit=2&offset=2");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement[]>();
        Assert.Equal(2, orders!.Length);
    }
}
