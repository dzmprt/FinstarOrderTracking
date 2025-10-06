using System.Net;
using System.Net.Http.Json;
using FOT.Application.UseCase.Orders.Commands.CreateOrder;

namespace FOT.WebApi.Tests.Orders;

public class CreateOrderTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("Test")]
    [InlineData("1")]
    public async Task Post_MustReturnCreated_WhenCreatedOrder(string description)
    {
        // Arrange
        const string url = "api/v1/orders";
        
        var command = new CreateOrderCommand
        {
            Description = description
        };
        var client = factory.CreateClient();
        
        // Act
        var response = await client.PostAsJsonAsync(url, command);

        // Assert
        var result = await response.Content.ReadAsStringAsync(CancellationToken.None);
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains(description, result);
    }
    
    [Fact]
    public async Task Post_MustReturnBadRequest_WhenIncorrectRequest()
    {
        // Arrange
        const string url = "api/v1/orders";
        
        var command = new CreateOrderCommand();
        var client = factory.CreateClient();
        
        // Act
        var response = await client.PostAsJsonAsync(url, command);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_MustReturnConflict_WhenReplayNonceError()
    {
        // Arrange
        const string url = "api/v1/orders";
        
        var command = new CreateOrderCommand
        {
            Description = "test"
        };
        var client = factory.CreateClient();
        var replayNonce = Guid.NewGuid().ToString();
        client.DefaultRequestHeaders.Add("Replay-Nonce", replayNonce);
        await client.PostAsJsonAsync(url, command);
        
        // Act
        var response = await client.PostAsJsonAsync(url, command);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}