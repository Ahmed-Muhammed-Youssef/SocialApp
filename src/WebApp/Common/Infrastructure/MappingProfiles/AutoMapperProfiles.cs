using Application.DTOs.Message;
using Application.DTOs.Picture;
using Application.DTOs.Post;
using Application.DTOs.Registeration;
using Application.DTOs.User;
using Domain.Entities;
using Infrastructure.Extensions;
using AutoMapper;

namespace Infrastructure.MappingProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Picture, PictureDTO>();
            CreateMap<UpdatedUserDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>()
                .ForMember(au => au.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(au => au.Email, opt => opt.MapFrom(src => src.Email));

            // @TODO: add profile picture mapping here
            CreateMap<Message, MessageDTO>().ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(m => m.Sender.Pictures.FirstOrDefault().Url));
            
            CreateMap<Post, PostDTO>();
            CreateMap<AddPostDTO, Post>();
        }

    }
}
