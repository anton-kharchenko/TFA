using TFA.Forum.Domain.Monitoring;

namespace TFA.Forum.Domain.Interfaces.Monitoring;

public interface IMonitoringRequest
{
    void MonitorSuccess(DomainMetrics metrics);
    void MonitorFailure(DomainMetrics metrics);
}