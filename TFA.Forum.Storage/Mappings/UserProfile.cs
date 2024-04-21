using AutoMapper;
using TFA.Forum.Domain.Share;
using TFA.Forum.Storage.Entities;

namespace TFA.Forum.Storage.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RecognisedUser>();
        CreateMap<Session, Forum.Domain.Authentication.Session>();
    }
}