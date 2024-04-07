using TFA.Domain.Monitoring;

namespace TFA.Domain.Interfaces.Monitoring;

public interface IMonitoringRequest
{
    void MonitorSuccess(DomainMetrics metrics);
}