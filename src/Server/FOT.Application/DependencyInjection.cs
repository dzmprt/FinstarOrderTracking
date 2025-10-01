using System.Reflection;
using FluentValidation;
using FOT.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using MitMediator;

namespace FOT.Application;

/// <summary>
/// Dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add application services.
    /// </summary>
    /// <param name="services"><see cref="IServiceProvider"/>.</param>
    /// <returns><see cref="IServiceProvider"/></returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddMitMediator()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true)
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(DatabaseTransactionBehavior<,>));
    }
}