namespace Application.Features.Users.GetUserPictureById;

public record GetUserPictureByIdQuery(int PictureId) : IQuery<Result<PictureDTO>>;