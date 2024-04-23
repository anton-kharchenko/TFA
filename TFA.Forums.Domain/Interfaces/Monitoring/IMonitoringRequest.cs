using TFA.Forums.Domain.Monitoring;

namespace TFA.Forums.Domain.Interfaces.Monitoring;

public interface IMonitoringRequest
{
    void MonitorSuccess(DomainMetrics metrics);
    void MonitorFailure(DomainMetrics metrics);
}