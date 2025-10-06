using System.Reflection;
using System.Text.Json.Serialization;
using FOT.WebApi.SwaggerFilters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FOT.WebApi;

/// <summary>
/// Inject ASP.NET services extensions.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add ASP.NET services.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    /// <returns><see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAspNetServices(this IServiceCollection services, IConfiguration configuration)
    {
        services = services
            .AddHttpContextAccessor()
            .AddResponseCompression(options => { options.EnableForHttps = true; })
            .Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new TrimmingConverter());
                // options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            })
            .AddSwagger(
                Assembly.GetExecutingAssembly(),
                configuration["AppInfo:AppName"]!,
                configuration["AppInfo:Version"]!,
                configuration["AppInfo:Description"]!)
            .AddDevCors();
        return services;
    }

    private static IServiceCollection AddDevCors(this IServiceCollection services)
    {
        return services
            .AddCors(options =>
            {
                options.AddPolicy(CorsPolicy.AllowAll, policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                    policy.WithExposedHeaders("*");
                });
            });
    }

    private static IServiceCollection AddSwagger(
        this IServiceCollection services,
        Assembly apiAssembly,
        string appName,
        string version,
        string description)
    {
        return services.AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.Name);

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Version = version,
                    Title = appName,
                    Description = description
                });

                options.UseInlineDefinitionsForEnums();
                options.UseAllOfToExtendReferenceSchemas();
                options.SupportNonNullableReferenceTypes();
                options.SchemaFilter<NonNullableSchemaFilter>();

                var xmlFilename = $"{apiAssembly.GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
    }
}
