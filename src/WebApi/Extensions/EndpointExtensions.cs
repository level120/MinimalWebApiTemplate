using WebApi.Endpoints.Interfaces;
using WebApi.Endpoints.V1;

namespace WebApi.Extensions;

/// <summary>
/// This class contains extension methods for the <see cref="IEndpointRouteBuilder"/> interface.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Registers custom endpoints for the application.
    /// This method adds all custom endpoints to the service collection.
    /// </summary>
    /// <param name="services">services</param>
    /// <returns>services</returns>
    public static IServiceCollection AddCustomEndpoints(this IServiceCollection services)
    {
        // Register all custom endpoints
        services.AddTransient<ICustomEndpoints, ErrorReportEndpoints>();

        return services;
    }

    /// <summary>
    /// Registers custom endpoints for the application.
    /// This method retrieves all registered <see cref="ICustomEndpoints"/> implementations
    /// from the service provider and calls their <see cref="ICustomEndpoints.ConfigureEndpoints(WebApplication)"/> method.
    /// </summary>
    /// <param name="app">app</param>
    public static void RegisterCustomEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<ICustomEndpoints>();

        foreach (var endpoint in endpoints)
        {
            endpoint.ConfigureEndpoints(app);
        }
    }
}
