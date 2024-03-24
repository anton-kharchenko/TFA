using AutoMapper;
using TFA.Api.Responses;
using TFA.Domain.Models;

namespace TFA.Api.Mappings;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Forum, ForumResponse>();
        CreateMap<Topic, TopicResponse>();
    }
}