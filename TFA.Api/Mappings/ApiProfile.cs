using AutoMapper;
using TFA.Api.Responses;
using TFA.Forum.Domain.Models;

namespace TFA.Api.Mappings;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Forum.Domain.Models.Forum, ForumResponse>();
        CreateMap<Topic, TopicResponse>();
    }
}