using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Mediator;
using Shared.Extensions;
using Shared.Results;

namespace Application.Features.Users.Update;

public class UpdateUserHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<UpdateUserCommand, Result<UserDTO>>
{
    public async ValueTask<Result<UserDTO>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        ApplicationUser? appUser = await unitOfWork.ApplicationUserRepository.GetByIdAsync(currentUserService.GetPublicId());

        if (appUser == null)
        {
            return Result<UserDTO>.NotFound();
        }

        appUser.FirstName = command.FirstName;
        appUser.LastName = command.LastName;
        appUser.Bio = command.Bio;
        appUser.CityId = command.CityId;

        unitOfWork.ApplicationUserRepository.Update(appUser);

        await unitOfWork.SaveChangesAsync();

        UserDTO userDto = new()
        {
            Id = appUser.Id,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            ProfilePictureUrl = appUser.ProfilePictureUrl,
            Sex = appUser.Sex,
            Age = appUser.DateOfBirth.CalculateAge(),
            Created = appUser.Created,
            LastActive = appUser.LastActive,
            Bio = appUser.Bio ?? string.Empty,
            Pictures = []
        };

        return Result<UserDTO>.Success(userDto);
    }
}
