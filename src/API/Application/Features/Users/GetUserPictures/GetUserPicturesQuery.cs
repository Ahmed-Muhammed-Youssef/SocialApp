namespace Application.Features.Users.GetUserPictures;

public record GetUserPicturesQuery : IQuery<Result<List<PictureDTO>>>;
