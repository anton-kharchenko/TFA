using AutoMapper;
using TFA.Forums.Api.Responses;
using TFA.Forums.Domain.Models;
using Comment = TFA.Forums.Domain.Models.Comment;

namespace TFA.Forums.Api.Mappings;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Forum, ForumResponse>();
        CreateMap<Topic, TopicResponse>();
        CreateMap<Comment, CommentResponse>();
    }
}