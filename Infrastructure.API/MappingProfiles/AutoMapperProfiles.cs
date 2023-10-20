using API.Application.DTOs;
using API.Domain.Entities;
using API.Infrastructure.Extensions;
using AutoMapper;
using System.Linq;

namespace API.Infrastructure.MappingProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Picture, PictureDTO>();
            CreateMap<UpdatedUserDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>();

            // @TODO: add profile picture mapping here
            CreateMap<Message, MessageDTO>().ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(m => m.Sender.Pictures.FirstOrDefault().Url));
        }

    }
}
