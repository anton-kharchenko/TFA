using AutoMapper;
using TFA.Forums.Storage.Entities;

namespace TFA.Forums.Storage.Mappings;

internal class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<Entities.Forum, Forums.Domain.Models.Forum>()
            .ForMember(d => d.Id,
                s => s.MapFrom(f => f.ForumId));
    }
}