using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using System.Linq;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<UpdatedUserDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>().ForMember(appUser => appUser.Password, opt => opt.Ignore());
            CreateMap<Message, MessageDTO>();
        }

    }
}
