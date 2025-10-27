using Application.Interfaces;
using Application.Interfaces.Services;
using Mediator;
using Shared.Results;

namespace Application.Features.Pictures.List;

public class GetPicturesHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IQueryHandler<GetPicturesQuery, Result<List<PictureDTO>>>
{
    public async ValueTask<Result<List<PictureDTO>>> Handle(GetPicturesQuery query, CancellationToken cancellationToken)
    {
        var pictures = await unitOfWork.PictureRepository.GetUserPictureDTOsAsync(currentUserService.GetPublicId());
        return Result<List<PictureDTO>>.Success(pictures);
    }
}
