namespace Application.Features.Users.GetUserPictures;

public class GetPicturesHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IQueryHandler<GetUserPicturesQuery, Result<List<PictureDTO>>>
{
    public async ValueTask<Result<List<PictureDTO>>> Handle(GetUserPicturesQuery query, CancellationToken cancellationToken)
    {
        var pictures = await unitOfWork.ApplicationUserRepository.GetUserPictureDTOsAsync(currentUserService.GetPublicId(), cancellationToken);
        return Result<List<PictureDTO>>.Success(pictures);
    }
}
