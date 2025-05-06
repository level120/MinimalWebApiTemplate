using WebApi.Services.Interfaces;

namespace WebApi.Services;

/// <summary>
/// BackendApiService is a service that provides methods to interact with the backend API.
/// It is designed to be used with dependency injection.
/// This class implements the <see cref="IBackendApiService"/> interface.
/// </summary>
public sealed class BackendApiService : IBackendApiService
{
    private readonly ILogger<BackendApiService> _logger;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackendApiService"/> class.
    /// </summary>
    /// <param name="logger">logger</param>
    /// <param name="httpClient">httpClient</param>
    public BackendApiService(ILogger<BackendApiService> logger, HttpClient httpClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        // Constructor logic can be added here if needed
    }
}
