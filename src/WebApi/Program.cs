using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.HttpLogging;

using Polly;

using WebApi.Contexts;
using WebApi.Extensions;
using WebApi.Services.Interfaces;

// Add logging for bootstrap.
LogConfigurationExtensions.ConfigureBootstrapLogging();

var builder = WebApplication.CreateSlimBuilder(args);

// Add logging to the application.
builder.ConfigureLogging();

// Configure JSON serialization options for the application.
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, OpenApiJsonSerializerContext.Default);
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add custom endpoints.
builder.Services.AddCustomEndpoints();

// Learn more about configuring HTTP logging at https://learn.microsoft.com/ko-kr/aspnet/core/fundamentals/http-requests?view=aspnetcore-9.0#use-polly-based-handlers
builder.Services.AddHttpClient<IBackendApiService>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, retryNumber => TimeSpan.FromMilliseconds(500 * Math.Pow(2, retryNumber))));

builder.Services.AddHttpLogging(
    logging => logging.LoggingFields = HttpLoggingFields.Duration |
                                       HttpLoggingFields.RequestMethod |
                                       HttpLoggingFields.RequestPath |
                                       HttpLoggingFields.RequestProtocol |
                                       HttpLoggingFields.ResponseStatusCode);

// Add custom services to the application.
builder.Services.AddCustomServices();

var app = builder.Build();

// Use logging.
app.UseConfiguredLogging();

// Register the custom endpoints.
app.RegisterCustomEndpoints();

// Use Exception Page for development, It's optional.
app.UseDeveloperExceptionPage();

// Use HTTP logging for all requests.
app.UseHttpLogging();

// Use OpenAPI for API documentation.
app.MapOpenApi();

app.Run();
