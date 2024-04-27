using AutoMapper;
using TFA.Forums.Storage.Entities;

namespace TFA.Forums.Storage.Mappings;

internal class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Topic, Forums.Domain.Models.Topic>()
            .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
        
        CreateMap<TopicListItemReadModel, Topic>()
            .ForMember(d => d.TopicId,
                s => s.MapFrom(f => f.TopicId));
    }
}