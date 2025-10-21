using Application.DTOs.Message;
using Application.DTOs.Picture;
using Application.DTOs.Post;
using Application.DTOs.User;
using Domain.Entities;
using AutoMapper;
using Shared.Extensions;

namespace Application.MappingProfiles;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<ApplicationUser, UserDTO>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

        CreateMap<Picture, PictureDTO>();
        
        CreateMap<UpdatedUserDTO, ApplicationUser>();
        
        // @TODO: add profile picture mapping here
        CreateMap<Message, MessageDTO>();
        
        CreateMap<Post, PostDTO>();
        CreateMap<AddPostDTO, Post>();
    }

}
