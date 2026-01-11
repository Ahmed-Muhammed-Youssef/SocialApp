namespace Application.Features.Users.GetUserPictureById;

public class GetUserPictureByIdHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IQueryHandler<GetUserPictureByIdQuery, Result<PictureDTO>>
{
    public async ValueTask<Result<PictureDTO>> Handle(GetUserPictureByIdQuery query, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetPublicId();
        PictureDTO? picture = await unitOfWork.PictureRepository.GetUserPictureDTOAsync(query.PictureId, userId, cancellationToken);
        if (picture == null)
        {
            return Result<PictureDTO>.Error($"Picture with id {query.PictureId} was not found.");
        }
        return Result<PictureDTO>.Success(picture);
    }
}
