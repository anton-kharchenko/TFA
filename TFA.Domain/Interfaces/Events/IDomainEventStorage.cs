using TFA.Domain.Interfaces.Storages;

namespace TFA.Domain.Interfaces.Events;

public interface IDomainEventStorage  : IStorage
{
    Task AddEventAsync<TDomainEntity>(TDomainEntity entity, CancellationToken cancellationToken);
}