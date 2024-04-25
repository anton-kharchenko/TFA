using TFA.Forums.Domain.Events;

namespace TFA.Forums.Domain.Interfaces.Storages;

public interface IDomainEventStorage : IStorage
{
    Task AddEventAsync(ForumDomainEvent domainEvent, CancellationToken cancellationToken);
}