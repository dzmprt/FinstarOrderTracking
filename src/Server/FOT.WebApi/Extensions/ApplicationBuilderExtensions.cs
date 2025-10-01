using FOT.WebApi.Middlewares;

namespace FOT.WebApi;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Use custom exceptions handler.
    /// </summary>
    /// <param name="builder"><see cref="IApplicationBuilder"/>.</param>
    /// <returns><see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseCustomExceptionsHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionsHandlerMiddleware>();
    }
    
    /// <summary>
    /// Use configured swagger.
    /// </summary>
    /// <param name="builder"><see cref="IApplicationBuilder"/>.</param>
    /// <returns><see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseConfiguredSwaggerUI(this IApplicationBuilder builder)
    {
        builder.UseSwagger(c => { c.RouteTemplate = "swagger/{documentname}/swagger.json"; })
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
            });
        return builder;
    }

    /// <summary>
    /// Use replay protection middleware.
    /// </summary>
    /// <param name="builder"><see cref="IApplicationBuilder"/>.</param>
    /// <returns><see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseReplayProtection(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ReplayProtectionMiddleware>();
    }
}