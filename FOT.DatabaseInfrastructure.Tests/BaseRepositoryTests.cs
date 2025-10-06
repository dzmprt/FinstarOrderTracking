using FOT.DatabaseInfrastructure.Implementations;
using FOT.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace FOT.DatabaseInfrastructure.Tests;

public class BaseRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlite("Data Source=:memory:") 
        .Options;

    private ApplicationDbContext CreateDbContext()
    {
        var context = new ApplicationDbContext(_dbContextOptions);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        await using var context = CreateDbContext();
        var repository = new BaseRepository<Order>(context);
        var order = new Order("Test", DateTimeOffset.UtcNow);
    
        await repository.AddAsync(order, CancellationToken.None);
    
        var result = await context.Orders.FindAsync(order.OrderNumber);
        Assert.NotNull(result);
        Assert.Equal(order.OrderNumber, result.OrderNumber);
    }
    
    [Fact]
    public async Task RemoveAsync_ShouldDeleteEntity()
    {
        await using var context = CreateDbContext();
        var repository = new BaseRepository<Order>(context);
        var order = new Order("test", DateTime.Now);
    
        context.Orders.Add(order);
        await context.SaveChangesAsync();
    
        await repository.RemoveAsync(order, CancellationToken.None);
    
        var result = await context.Orders.FindAsync(order.OrderNumber);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task FirstOrDefaultAsync_ShouldReturnEntity_WhenExists()
    {
        await using var context = CreateDbContext();
        var repository = new BaseRepository<Order>(context);
        var order = new Order("test", DateTime.Now);
    
        context.Orders.Add(order);
        await context.SaveChangesAsync();
    
        var result = await repository.FirstOrDefaultAsync(o => order.OrderNumber == o.OrderNumber, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(order.OrderNumber, result.OrderNumber);
    }
    
    [Fact]
    public async Task SearchAsync_ShouldReturnEntities()
    {
        await using var context = CreateDbContext();
        var repository = new BaseRepository<Order>(context);
        context.Orders.AddRange(
            new Order("SearchAsync Test 1", DateTime.Now),
            new Order("SearchAsync Test 2", DateTime.Now)
        );
        await context.SaveChangesAsync();
    
        var result = await repository.SearchAsync<Order>(c => c.Description.StartsWith("SearchAsync Test"), null, null, null, CancellationToken.None);
        Assert.Equal(2, result.Length);
    }
}