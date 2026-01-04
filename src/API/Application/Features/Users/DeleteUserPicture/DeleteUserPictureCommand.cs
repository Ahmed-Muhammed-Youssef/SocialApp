namespace Application.Features.Users.DeleteUserPicture;

public record DeleteUserPictureCommand(int PictureId) : ICommand<Result<object?>>;
