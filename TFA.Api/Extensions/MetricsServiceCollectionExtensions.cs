using OpenTelemetry.Metrics;

namespace TFA.Api.Extensions;

internal static class MetricsServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(build => build
                .AddInstrumentation(serviceProvider => serviceProvider)
                .AddPrometheusExporter()
                .AddMeter("TFA.Domain")
                .AddConsoleExporter()
                .AddView("http.server.request.duration", new ExplicitBucketHistogramConfiguration()
                {
                    Boundaries = [0, 0.05, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 10]
                })
            );

        return services;
    }
}