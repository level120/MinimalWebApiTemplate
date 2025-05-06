using WebApi.Endpoints.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Endpoints.V1;

internal sealed class ErrorReportEndpoints : ICustomEndpoints
{
    private readonly ILogger<ErrorReportEndpoints> _logger;
    private readonly IBackendApiService _backendApiService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorReportEndpoints"/> class.
    /// </summary>
    /// <param name="httpClient">httpClient</param>
    /// <param name="logger">logger</param>
    public ErrorReportEndpoints(ILogger<ErrorReportEndpoints> logger, IBackendApiService backendApiService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _backendApiService = backendApiService ?? throw new ArgumentNullException(nameof(backendApiService));
    }

    /// <inheritdoc />
    public void ConfigureEndpoints(WebApplication app)
    {
        _logger.LogInformation("Configuring ErrorReport endpoints");

        // Register the endpoints
        app.MapGet("/api/v1/report/{id:int}", GetErrorReport);

        _logger.LogInformation("ErrorReport endpoints configured");
    }

    private async Task<IResult> GetErrorReport(int id)
    {
        await Task.Delay(1000).ConfigureAwait(false);
        _logger.LogInformation("Getting error report with ID: {Id}", id);

        return Results.Ok($"Error report with ID: {id}");
    }
}
