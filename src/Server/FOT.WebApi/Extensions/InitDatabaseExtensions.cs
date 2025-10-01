using FOT.Application.Abstractions;
using FOT.DatabaseInfrastructure;
using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;
using Microsoft.EntityFrameworkCore;

namespace FOT.WebApi.Extensions;

public static class InitDbExtensions
{
    public static WebApplication UpdateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var migrationAttemptsCount = 0;
        while (dbContext.Database.GetPendingMigrations().Any())
        {
            migrationAttemptsCount++;
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                if (migrationAttemptsCount == 10)
                    throw;

                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogWarning(ex, "Migration attempt failed: {Message}", ex.Message);
                Thread.Sleep(2000);
            }
        }

        return app;
    }
}