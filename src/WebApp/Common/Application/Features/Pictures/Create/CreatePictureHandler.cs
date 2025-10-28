using Application.Common.Interfaces;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Mediator;
using Shared.Results;

namespace Application.Features.Pictures.Create;

public class CreatePictureHandler(IUnitOfWork unitOfWork, IPictureService pictureService, ICurrentUserService currentUserService) : ICommandHandler<CreatePictureCommand, Result<int>>
{
    public async ValueTask<Result<int>> Handle(CreatePictureCommand command, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(currentUserService.GetPublicId());

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
            AppUserId = user.Id,
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        picture = await unitOfWork.PictureRepository.AddPictureAsync(picture);

        await unitOfWork.SaveChangesAsync();

        return Result<int>.Created(picture.Id);
    }
}
