using Domain.Entities;
using AutoMapper;
using Shared.Extensions;
using Application.Features.Posts;
using Application.Features.Users;
using Application.Features.Messages;
using Application.Features.Pictures;

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
    }

}
