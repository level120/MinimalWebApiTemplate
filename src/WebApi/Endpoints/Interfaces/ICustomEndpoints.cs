namespace WebApi.Endpoints.Interfaces;

/// <summary>
/// This interface defines a contract for custom endpoints in a web application.
/// Implementing classes should provide a method to configure the endpoints.
/// </summary>
public interface ICustomEndpoints
{
    /// <summary>
    /// Configures the endpoints for the application.
    /// </summary>
    /// <param name="app">The web application builder.</param>
    void ConfigureEndpoints(WebApplication app);
}