using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TFA.Search.API.Monitoring;

internal static class MetricsServiceCollectionExtensions
{
    public static void AddApiMetrics(this IServiceCollection services,
        IConfiguration configuration)
    {
        var searchEnginHost = configuration.GetConnectionString("SearchEngine");
        
        services
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddInstrumentation(serviceProvider => serviceProvider)
                .AddPrometheusExporter()
                .AddMeter("TFA.Forums.Domain")
            )
            .WithTracing(builder => builder.ConfigureResource(r => r.AddService("TFA"))
                .AddAspNetCoreInstrumentation(opt =>
                {
                    opt.Filter += context =>
                        !context.Request.Path.Value!.Contains("metrics", StringComparison.InvariantCulture) &&
                        !context.Request.Path.Value!.Contains("swagger", StringComparison.InvariantCulture);
                    opt.EnrichWithHttpResponse = (activity, response) =>
                        activity.AddTag("error", response.StatusCode = 400);
                })
                .AddHttpClientInstrumentation(options =>
                {
                    options.FilterHttpRequestMessage += message => message.RequestUri!.ToString().Contains(searchEnginHost!);
                })
                .AddSource("TFA.Forums.Domain")
                .AddConsoleExporter()
                .AddJaegerExporter(options => options.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!))
            );
    }
}