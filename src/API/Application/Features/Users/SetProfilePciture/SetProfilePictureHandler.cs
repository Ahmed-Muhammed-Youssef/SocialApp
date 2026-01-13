namespace Application.Features.Users.SetProfilePciture;

public class SetProfilePictureHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<SetProfilePictureCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(SetProfilePictureCommand command, CancellationToken cancellationToken)
    {
        int userId = currentUserService.GetPublicId();

        var nChanges = await unitOfWork.ApplicationUserRepository.SetProfilePictureIfOwnedAsync(userId, command.PictureId, cancellationToken);

        if(nChanges == 0)
        {
            return Result<object?>.Error("Picture not found or does not belong to the user.");
        }

        return Result<object?>.Success(null);
    }
}
