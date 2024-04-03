using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace TFA.Domain.Monitoring;

public class DomainMetrics(IMeterFactory meterFactory)
{
    private Meter _meter = meterFactory.Create("TFA.Domain");
    

    private readonly ConcurrentDictionary<string, Counter<int>> _counters = new();
    
    public void ForumsFetched(bool success)
    {
        IncrementCounter("forum.fetched", 1, new Dictionary<string, object?>()
        {
            ["success"]  = success
        });
    }

    private void IncrementCounter(string key, int value, IReadOnlyDictionary<string, object?>? dictionary = null)
    {
        var counter = _counters.GetOrAdd(key, _=> _meter.CreateCounter<int>(key));
        counter.Add(value, dictionary?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }   
}