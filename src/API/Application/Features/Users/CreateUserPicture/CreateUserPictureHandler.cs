namespace Application.Features.Users.CreateUserPicture;

public class CreateUserPictureHandler(IUnitOfWork unitOfWork, IPictureService pictureService, ICurrentUserService currentUserService) : ICommandHandler<CreateUserPictureCommand, Result<int>>
{
    public async ValueTask<Result<int>> Handle(CreateUserPictureCommand command, CancellationToken cancellationToken)
    {
        if (command.File == null)
        {
            return Result<int>.Error("No file provided.");
        }

        ImageUploadResult uploadResult = await pictureService.AddPictureAsync(command.File);
        if (uploadResult == null || uploadResult.Error != null)
        {
            return Result<int>.Error(uploadResult?.Error?.Message ?? "Failed to upload image.");
        }

        var picture = Picture.Create(uploadResult.SecureUrl.AbsoluteUri, uploadResult.PublicId);

        unitOfWork.PictureRepository.Add(picture);

        await unitOfWork.CommitAsync(cancellationToken);

        var userId = currentUserService.GetPublicId();

        var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return Result<int>.Error("User not found.");
        }

        user.AddPicture(picture.Id);

        await unitOfWork.CommitAsync(cancellationToken);

        return Result<int>.Created(picture.Id);
    }
}
