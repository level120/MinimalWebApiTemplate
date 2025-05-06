using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi.Extensions;

/// <summary>
/// This class contains extension methods for the <see cref="IServiceCollection"/> interface.
/// It provides methods to register custom services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers custom services for the application.
    /// This method adds all custom services to the service collection.
    /// </summary>
    /// <param name="services">services</param>
    /// <returns>services</returns>
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        // Register your custom services here
        // Example: services.AddScoped<IMyService, MyService>();
        services.AddSingleton<IBackendApiService, BackendApiService>();

        return services;
    }
}
