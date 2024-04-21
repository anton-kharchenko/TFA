using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace TFA.Forum.Domain.Monitoring;

public class DomainMetrics(IMeterFactory meterFactory)
{
    private readonly Meter _meter = meterFactory.Create("TFA.Forum.Domain");
    
    private readonly ConcurrentDictionary<string, Counter<int>> _counters = new();
    
    internal static readonly ActivitySource ActivitySource = new ("TFA.Forum.Domain");
    
    public void IncrementCounter(string key, int value, IReadOnlyDictionary<string, object?>? dictionary = null)
    {
        var counter = _counters.GetOrAdd(key, _=> _meter.CreateCounter<int>(key));
        counter.Add(value, dictionary?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }
    
    public static IReadOnlyDictionary<string, object?> ResultTags(bool success) => new Dictionary<string, object?>
    {
        ["success"] = success
    };
}