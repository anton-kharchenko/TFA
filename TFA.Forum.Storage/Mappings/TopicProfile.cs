using AutoMapper;
using TFA.Forum.Storage.Entities;

namespace TFA.Forum.Storage.Mappings;

internal class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Topic, Forum.Domain.Models.Topic>()
            .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
    }
}