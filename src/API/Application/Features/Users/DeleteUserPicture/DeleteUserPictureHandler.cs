namespace Application.Features.Users.DeleteUserPicture;

public class DeleteUserPictureHandler(IUnitOfWork unitOfWork, IPictureService pictureService, ICurrentUserService currentUserService) : ICommandHandler<DeleteUserPictureCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(DeleteUserPictureCommand command, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(currentUserService.GetPublicId(), cancellationToken);

        if (user == null)
        {
            return Result<object?>.Unauthorized();
        }

        var pictures = await unitOfWork.PictureRepository.GetUserPictureAsync(user.Id);
        var picture = pictures.FirstOrDefault(p => p.Id == command.PictureId);
        if (picture == null)
        {
            return Result<object?>.NotFound($"Picture {command.PictureId} doesn't exist or is not owned by the user.");
        }

        var result = await pictureService.DeletePictureAsync(picture.PublicId);
        if (result == null || result.Error != null)
        {
            return Result<object?>.Error(result?.Error?.Message ?? "Failed to delete image from storage.");
        }

        unitOfWork.PictureRepository.Delete(picture);

        await unitOfWork.CommitAsync(cancellationToken);

        return Result<object?>.NoContent();
    }
}
