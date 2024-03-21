using AutoMapper;
using TFA.Domain.Share;
using TFA.Storage.Models;

namespace TFA.Storage.Mappings;

public class UserProfile : Profile
{
    public UserProfile() => CreateMap<User, RecognisedUser>();
}