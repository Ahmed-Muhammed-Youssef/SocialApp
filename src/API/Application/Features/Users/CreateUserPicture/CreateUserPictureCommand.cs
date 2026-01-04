namespace Application.Features.Users.CreateUserPicture;

public record CreateUserPictureCommand(IFormFile File) : ICommand<Result<int>>;
