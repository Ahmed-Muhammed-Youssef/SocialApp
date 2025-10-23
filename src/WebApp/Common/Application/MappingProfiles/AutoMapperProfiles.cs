using Application.DTOs.Message;
using Application.DTOs.Picture;
using Application.DTOs.Post;
using Domain.Entities;
using AutoMapper;
using Shared.Extensions;
using Application.Features.Posts;
using Application.Features.Users;

namespace Application.MappingProfiles;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<ApplicationUser, UserDTO>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

        CreateMap<Picture, PictureDTO>();
        
        // @TODO: add profile picture mapping here
        CreateMap<Message, MessageDTO>();
        
        CreateMap<Post, PostDTO>();
        CreateMap<CreatePostDTO, Post>();
    }

}
