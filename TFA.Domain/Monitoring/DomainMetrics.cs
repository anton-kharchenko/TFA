using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace TFA.Domain.Monitoring;

public class DomainMetrics(IMeterFactory meterFactory)
{
    private readonly Meter _meter = meterFactory.Create("TFA.Domain");
    
    private readonly ConcurrentDictionary<string, Counter<int>> _counters = new();
    
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