namespace Application.Features.Pictures.Create;

public class CreatePictureHandler(IUnitOfWork unitOfWork, IPictureService pictureService, ICurrentUserService currentUserService) : ICommandHandler<CreatePictureCommand, Result<int>>
{
    public async ValueTask<Result<int>> Handle(CreatePictureCommand command, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(currentUserService.GetPublicId(), cancellationToken);

        if (user is null)
        {
            return Result<int>.Unauthorized();
        }

        ImageUploadResult result = await pictureService.AddPictureAsync(command.File);
        if (result.Error != null)
        {
            return Result<int>.Error(result.Error.Message);
        }

        Picture picture = new()
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        unitOfWork.PictureRepository.Add(picture);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result<int>.Created(picture.Id);
    }
}
