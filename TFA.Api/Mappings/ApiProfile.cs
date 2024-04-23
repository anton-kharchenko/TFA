using AutoMapper;
using TFA.Api.Responses;
using TFA.Forums.Domain.Models;

namespace TFA.Api.Mappings;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Forums.Domain.Models.Forum, ForumResponse>();
        CreateMap<Topic, TopicResponse>();
    }
}