using FOT.DatabaseInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace FOT.WebApi.Extensions;

/// <summary>
/// Init database, run pending migrations.
/// </summary>
public static class InitDbExtensions
{
    /// <summary>
    /// Init database, run pending migrations.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
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
                {
                    throw;
                }

                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogWarning(ex, "Migration attempt failed: {Message}", ex.Message);
                Thread.Sleep(2000);
            }
        }

        return app;
    }
}
