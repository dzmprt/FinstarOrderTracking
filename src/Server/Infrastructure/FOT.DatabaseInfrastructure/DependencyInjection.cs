using FOT.Application.Abstractions;
using FOT.Application.Common.Abstractions.Persistence;
using FOT.DatabaseInfrastructure.Implementations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FOT.DatabaseInfrastructure;

/// <summary>
/// Dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add database services.
    /// </summary>
    /// <param name="services"><see cref="IServiceProvider"/>.</param>
    /// <param name="optionsAction">Entity framework configuration options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder>? optionsAction)
    {
        return services.AddDbContext<DbContext, ApplicationDbContext>(optionsAction)
            .AddScoped<IDatabaseTransactionCreator, DatabaseTransactionCreator>()
            .AddTransient(typeof(IBaseProvider<>), typeof(BaseProvider<>))
            .AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
    }
}