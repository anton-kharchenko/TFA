using AutoMapper;
using TFA.Domain.Share;
using TFA.Storage.Entities;

namespace TFA.Storage.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RecognisedUser>();
        CreateMap<Session, Domain.Authentication.Session>();
    }
}