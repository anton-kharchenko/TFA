using AutoMapper;
using TFA.Forums.Domain.Events;

namespace TFA.Forums.Storage.Mappings;

public class DomainEventsProfile : Profile
{
    public DomainEventsProfile()
    {
        CreateMap<ForumDomainEvent, Entities.ForumDomainEvent>();
        CreateMap<ForumDomainEvent.ForumComment, Entities.ForumDomainEvent.ForumComment>();
    }
}