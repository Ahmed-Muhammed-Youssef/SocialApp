namespace Application.Features.Pictures.Create;

public class CreatePictureHandler(IUnitOfWork unitOfWork, IPictureService pictureService, ICurrentUserService currentUserService) : ICommandHandler<CreatePictureCommand, Result<int>>
{
    public async ValueTask<Result<int>> Handle(CreatePictureCommand command, CancellationToken cancellationToken)
    {
        if (command.File == null)
            return Result<int>.Error("No file provided.");

        ImageUploadResult uploadResult = await pictureService.AddPictureAsync(command.File);
        if (uploadResult == null || uploadResult.Error != null)
        {
            return Result<int>.Error(uploadResult?.Error?.Message ?? "Failed to upload image.");
        }


        Picture picture = new()
        {
            Url = uploadResult.SecureUrl.AbsoluteUri,
            PublicId = uploadResult.PublicId
        };

        unitOfWork.PictureRepository.Add(picture);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result<int>.Created(picture.Id);
    }
}
