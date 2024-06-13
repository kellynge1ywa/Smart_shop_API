using AutoMapper;

namespace duka;

public class AppProfiles:Profile
{
    public AppProfiles()
    {
        // CreateMap<RegisterUserDto,User>().ForMember(dest=> dest.UserName, src=>src.MapFrom(email=> email.Email));
        CreateMap<RegisterUserDto, User>().ReverseMap();
    }

}
