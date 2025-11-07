namespace Application.Features.Users.Update;

public class UpdateUserHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<UpdateUserCommand, Result<UserDTO>>
{
    public async ValueTask<Result<UserDTO>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        ApplicationUser? appUser = await unitOfWork.ApplicationUserRepository.GetByIdAsync(currentUserService.GetPublicId(), cancellationToken);

        if (appUser == null)
        {
            return Result<UserDTO>.NotFound();
        }

        appUser.FirstName = command.FirstName;
        appUser.LastName = command.LastName;
        appUser.Bio = command.Bio;
        appUser.CityId = command.CityId;

        await unitOfWork.ApplicationUserRepository.UpdateAsync(appUser, cancellationToken);

        return Result<UserDTO>.Success(UserMappings.ToDto(appUser));
    }
}
