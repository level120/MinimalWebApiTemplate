using System.Diagnostics;

using Serilog;
using Serilog.Events;

namespace WebApi.Extensions;

/// <summary>
/// This class contains extension methods for configuring logging in the application.
/// </summary>
public static class LogConfigurationExtensions
{
    /// <summary>
    /// Configures logging for the application.
    /// </summary>
    public static void ConfigureBootstrapLogging()
    {
        // Add logging before the application starts.
        Log.Logger = new LoggerConfiguration()
            .ConfigureLogging()
            .CreateBootstrapLogger();
    }

    /// <summary>
    /// Configures logging for the application.
    /// This method sets up logging to use Serilog and configures it to log to the console and a file.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        // Add logging to the application using Serilog.
        builder.Services.AddSerilog((service, configuration) => configuration.ConfigureLogging(builder, service));
    }

    /// <summary>
    /// Configures logging for the application.
    /// This method sets up logging to use Serilog and configures it to log to the console and a file.
    /// It also enriches the logs with additional properties such as application name, environment, framework, and version.
    /// The logs are written to the console and a file in the LogFiles directory.
    /// The file is rolled daily and retains the last 7 files.
    /// The file size limit is set to 350 MB, and the logs are flushed to disk every second.
    /// </summary>
    /// <param name="configuration">configuration</param>
    /// <param name="builder">builder</param>
    /// <param name="service">service</param>
    public static LoggerConfiguration ConfigureLogging(
        this LoggerConfiguration configuration, WebApplicationBuilder? builder = null, IServiceProvider? service = null)
    {
        var programName = string.IsNullOrEmpty(Environment.ProcessPath)
            ? Process.GetCurrentProcess().MainModule!.ModuleName
            : Path.GetFileName(Environment.ProcessPath);
        var programPath = Path.Combine(AppContext.BaseDirectory, programName);
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(programPath)?.FileVersion ?? "Unknown";

        Log.Logger?.Information("Configure Info:");
        Log.Logger?.Information($"\t* Framework: {AppContext.TargetFrameworkName}");
        Log.Logger?.Information($"\t* Program: {programName}");
        Log.Logger?.Information($"\t* Path: {programPath}");
        Log.Logger?.Information($"\t* Version: {fileVersionInfo}");

        if (builder is not null)
        {
            configuration.ReadFrom.Configuration(builder.Configuration);
        }

        if (service is not null)
        {
            configuration.ReadFrom.Services(service);
        }

        return configuration.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
                            .Enrich.FromLogContext()
                            .Enrich.WithProperty("ApplicationName", "WebApi")
                            .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production")
                            .Enrich.WithProperty("Framework", AppContext.TargetFrameworkName)
                            .Enrich.WithProperty("Version", fileVersionInfo)
                            .WriteTo.Console()
                            .WriteTo.Debug()
                            .WriteTo.File(
                                Path.Combine(AppContext.BaseDirectory, "LogFiles", "diagnostics-.log"),
                                rollingInterval: RollingInterval.Day,
                                retainedFileCountLimit: 7,
                                rollOnFileSizeLimit: true,
                                fileSizeLimitBytes: 350 * 1024 * 1024, // 350 MB
                                shared: true,
                                flushToDiskInterval: TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// /// Configures logging for the application.
    /// </summary>
    /// <param name="app">app</param>
    public static void UseConfiguredLogging(this WebApplication app)
    {
        // Use Serilog for logging.
        app.UseSerilogRequestLogging();
    }
}
