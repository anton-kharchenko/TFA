using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using TFA.Forums.Domain.Interfaces.Storages;
using TFA.Forums.Storage.Configurations;
using TFA.Forums.Storage.Entities;
using TFA.Forums.Storage.Interfaces;
using Events_ForumDomainEvent = TFA.Forums.Domain.Events.ForumDomainEvent;

namespace TFA.Forums.Storage.Storages;

internal class DomainEventStorage(
    ForumDbContext dbContext,
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    IMapperBase mapper) : IDomainEventStorage
{
    public async Task AddEventAsync(Events_ForumDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var storageDomainEvent = mapper.Map<ForumDomainEvent>(domainEvent);

        await dbContext.DomainEvents.AddAsync(new DomainEvent
        {
            DomainEventId = guidFactory.Create(),
            EmittedAt = momentProvider.Now,
            ContentBlob = JsonSerializer.SerializeToUtf8Bytes(storageDomainEvent),
            ActivityId = Activity.Current?.Id
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}