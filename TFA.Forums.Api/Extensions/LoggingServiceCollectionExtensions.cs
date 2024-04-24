using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace TFA.Forums.Api.Extensions;

internal static class LoggingServiceCollectionExtensions
{
    private static readonly string[] PropertiesAsLabels =
    [
        "level", "Environment", "Application", "SourceContext"
    ];

    public static void AddApiLogging(this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var loggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);
        services.AddSingleton(loggingLevelSwitch);
        
        services.AddLogging(loggingBuilder => loggingBuilder
            .Configure(options => options.ActivityTrackingOptions =
                ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId)
            .AddSerilog(new LoggerConfiguration()
                .MinimumLevel.ControlledBy(loggingLevelSwitch)
                .Enrich.WithProperty("Application", "TFA.Forums.API")
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .Enrich.With<TracingContextEnricher>()
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.GrafanaLoki(configuration.GetConnectionString("Logs")!,
                        propertiesAsLabels: PropertiesAsLabels, leavePropertiesIntact: true))
                .WriteTo.Logger(lc => lc.WriteTo.Console())
                .CreateLogger()));
    }

    private class  TracingContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;
            if(activity is null) return;
            
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId",  activity.TraceId));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity.SpanId));
        }
    }
}