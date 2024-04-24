using AutoMapper;
using TFA.Forums.Api.Responses;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Api.Mappings;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Forums.Domain.Models.Forum, ForumResponse>();
        CreateMap<Topic, TopicResponse>();
    }
}