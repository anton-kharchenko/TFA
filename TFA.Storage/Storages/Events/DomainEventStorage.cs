using System.Text.Json;
using TFA.Domain.Interfaces.Events;
using TFA.Storage.Context;
using TFA.Storage.Entities;
using TFA.Storage.Interfaces;

namespace TFA.Storage.Storages.Events;

public class DomainEventStorage(
    ForumDbContext dbContext,
    IGuidFactory guidFactory,
    IMomentProvider momentProvider) : IDomainEventStorage
{
    public async Task AddEventAsync<TDomainEntity>(TDomainEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.DomainEvents.AddAsync(new DomainEvent
        {
            DomainEventId = guidFactory.Create(),
            EmittedAt = momentProvider.Now,
            ContentBlob = JsonSerializer.SerializeToUtf8Bytes(entity)
        }, cancellationToken: cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}