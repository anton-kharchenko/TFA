using AutoMapper;
using TFA.Storage.Models;

namespace TFA.Storage.Mappings;

internal class TopicProfile : Profile
{
    public TopicProfile() =>
        CreateMap<Topic, Domain.Models.Topic>()
            .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
}