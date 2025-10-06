using FOT.Application.Common.Abstractions.Infrastructure.Database;
using FOT.Application.UseCase.Orders.Commands.CreateOrder;
using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;
using Moq;

namespace FOT.Application.Tests;

public class CreateOrderCommandTests
{
    [Fact]
    public async Task Handler_CreatesOrder_ReturnsDto()
    {
        var repo = new Mock<IBaseRepository<Order>>();
        repo.Setup(r => 
                r.AddAsync(It.IsAny<Order>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Order("123", DateTimeOffset.UtcNow));
        
        var handler = new CreateOrderCommandHandler(repo.Object);
        var cmd = new CreateOrderCommand { Description = "desc" };
        var result = await handler.HandleAsync(cmd, CancellationToken.None);
        Assert.Equal("desc", result.Description);
        Assert.Equal(OrderStatus.Created, result.Status);
        Assert.Null(result.UpdatedAt);
    }

    [Fact]
    public void Validator_ValidDescription_Passes()
    {
        var validator = new CreateOrderCommandValidator();
        var cmd = new CreateOrderCommand { Description = "desc" };
        var result = validator.Validate(cmd);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(
        "a9Kf3Lz7Qw2Xn8Bv5Tg0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4Ea9Kj3Lf7Qx2Zn8Bw5Tv0Rm6Yc1Ue4Jo9Hd3Pq7Zx2Wl8Ns5Ct0Vb6Ay1Ek4Jm9Hg3Lq7Xz2Wn8Bt5Cv0Ru6Yd1Up4E")]
    public void Validator_InvalidDescription_Fails(string desc)
    {
        var validator = new CreateOrderCommandValidator();
        var cmd = new CreateOrderCommand { Description = desc };
        var result = validator.Validate(cmd);
        Assert.False(result.IsValid);
    }
}