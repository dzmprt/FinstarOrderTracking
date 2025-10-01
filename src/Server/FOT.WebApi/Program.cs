using FOT.Application;
using FOT.DatabaseInfrastructure;
using FOT.WebApi;
using FOT.WebApi.Endpoints;
using FOT.WebApi.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var useInMemorySqlLite = builder.Configuration.GetValue<bool>("UseInMemorySqlLite");

builder.Host.UseSerilog();
builder.Services
    .AddAspNetServices(builder.Configuration)
    .AddApplicationServices()
    .AddDatabaseServices(
        useInMemorySqlLite
            ? options =>
            {
                var connection = new SqliteConnection("Data Source=:memory:");
                connection.Open();
                options.UseSqlite(connection);
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlite(connection);
                using var context = new ApplicationDbContext(optionsBuilder.Options);
                context.Database.Migrate();
            }
            : options => { options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); }
    )
    .AddMemoryCache();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseCors(CorsPolicy.AllowAll);
}
else
{
    app.UseHttpsRedirection();
}


app.UseCustomExceptionsHandler()
    .UseConfiguredSwaggerUI()
    .UseReplayProtection();

if (!useInMemorySqlLite)
{
    app.UpdateDatabase();
}

app.RegisterAppInfoEndpoints()
    .UseOrdersApi();

app.Run();