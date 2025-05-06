using WebApi.Endpoints.Interfaces;

namespace WebApi.Endpoints.V1;

internal sealed class ErrorReportEndpoints : ICustomEndpoints
{
    private readonly ILogger<ErrorReportEndpoints> _logger;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorReportEndpoints"/> class.
    /// </summary>
    /// <param name="httpClient">httpClient</param>
    /// <param name="logger">logger</param>
    public ErrorReportEndpoints(HttpClient httpClient, ILogger<ErrorReportEndpoints> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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
