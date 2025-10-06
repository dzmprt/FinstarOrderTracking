using Microsoft.AspNetCore.Mvc;

namespace FOT.WebApi.Extensions;

internal static class WebApplicationExtensions
{
    /// <summary>
    /// Register app info endpoints.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication RegisterAppInfoEndpoints(this WebApplication app)
    {
        app.MapGet("app-info/version",
                ([FromServices] IConfiguration configuration) => Results.Ok(new AppInfo(configuration).Version))
            .WithName("Version")
            .WithDescription("Version")
            .WithTags("App info")
            .WithOpenApi();

        app.MapGet("app-info/who-am-i",
                ([FromServices] IConfiguration configuration) => Results.Ok(new AppInfo(configuration)))
            .WithName("WhoAmI")
            .WithDescription("Who am i")
            .WithTags("App info")
            .WithOpenApi();

        return app;
    }
}

internal record struct AppInfo
{
    public string Version { get; private set; }
        
    public string AppName { get; private set; }
        
    public string Description { get; private set; }

    public AppInfo(IConfiguration configuration)
    {
        Version = configuration["AppInfo:Version"]!;
        AppName = configuration["AppInfo:AppName"]!;
        Description = configuration["AppInfo:Description"]!;
    }
}