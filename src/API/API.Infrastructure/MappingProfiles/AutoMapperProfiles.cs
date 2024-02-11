using API.Application.DTOs.Message;
using API.Application.DTOs.Picture;
using API.Application.DTOs.Post;
using API.Application.DTOs.Registeration;
using API.Application.DTOs.User;
using API.Domain.Entities;
using API.Infrastructure.Extensions;
using AutoMapper;

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
            
            CreateMap<Post, PostDTO>();
            CreateMap<AddPostDTO, Post>();
        }

    }
}
