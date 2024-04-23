using AutoMapper;
using TFA.Forums.Domain.Share;
using TFA.Forums.Storage.Entities;

namespace TFA.Forums.Storage.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RecognisedUser>();
        CreateMap<Session, Forums.Domain.Authentication.Session>();
    }
}