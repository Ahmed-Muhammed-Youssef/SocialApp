namespace Application.Features.Pictures.List;

public record GetPicturesQuery : IQuery<Result<List<PictureDTO>>>;
