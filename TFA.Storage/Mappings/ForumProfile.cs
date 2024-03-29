﻿using AutoMapper;
using TFA.Storage.Entities;

namespace TFA.Storage.Mappings;

internal class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<Forum, Domain.Models.Forum>()
            .ForMember(d => d.Id,
                s => s.MapFrom(f => f.ForumId));
    }
}