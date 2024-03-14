using AutoMapper;
using TFA.Domain.Models;

namespace TFA.Api.Mappings;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Forum, Responses.ForumResponse>();
        CreateMap<Topic, Responses.TopicResponse>();
    }
}