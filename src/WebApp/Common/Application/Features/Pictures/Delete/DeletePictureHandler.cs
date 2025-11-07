namespace Application.Features.Pictures.Delete;

public class DeletePictureHandler(IUnitOfWork unitOfWork, IPictureService pictureService, ICurrentUserService currentUserService) : ICommandHandler<DeletePictureCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(DeletePictureCommand command, CancellationToken cancellationToken)
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
            return Result<object?>.NotFound($"{command.PictureId} doesn't exist.");
        }
        if (picture.AppUserId != user.Id)
        {
            return Result<object?>.Unauthorized();
        }
        var result = await pictureService.DeletePictureAsync(picture.PublicId);
        if (result.Error != null)
        {
            return Result<object?>.Error(result.Error.Message);
        }

        await unitOfWork.PictureRepository.DeleteAsync(picture, cancellationToken);

        //@TODO: what if the image is a profile picture
        
        return Result<object?>.NoContent();
    }
}
