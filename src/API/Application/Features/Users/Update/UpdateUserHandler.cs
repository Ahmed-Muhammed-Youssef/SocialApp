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

        appUser.Update(command.FirstName, command.LastName, command.CityId, command.Bio);

        unitOfWork.ApplicationUserRepository.Update(appUser);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result<UserDTO>.Success(UserMappings.ToDto(appUser));
    }
}
