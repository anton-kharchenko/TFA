using AutoMapper;
using TFA.Forum.Storage.Entities;

namespace TFA.Forum.Storage.Mappings;

internal class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<Entities.Forum, Forum.Domain.Models.Forum>()
            .ForMember(d => d.Id,
                s => s.MapFrom(f => f.ForumId));
    }
}