using System.Reflection;
using FOT.Application.Common.Notifications;
using FOT.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace FOT.DatabaseInfrastructure;

/// <summary>
/// Application Db context.
/// </summary>
public class ApplicationDbContext: DbContext
{
    /// <summary>
    /// Orders DbSet.
    /// </summary>
    internal DbSet<Order> Orders { get; set; }
    
    /// <summary>
    /// DomainEventOutboxNotification DbSet.
    /// </summary>
    internal DbSet<DomainEventOutbox> DomainEventOutboxNotifications { get; set; }
    
    /// <summary>
    /// Initializes a new instance of <see cref="ApplicationDbContext"/>.
    /// </summary>
    /// <param name="options"><see cref="DbContextOptions"/></param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        if (Database.ProviderName?.Contains("Npgsql") == true)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        property.SetColumnType("citext");
                    }
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}