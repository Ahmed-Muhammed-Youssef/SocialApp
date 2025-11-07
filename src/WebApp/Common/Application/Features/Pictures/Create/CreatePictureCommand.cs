namespace Application.Features.Pictures.Create;

public record CreatePictureCommand(IFormFile File) : ICommand<Result<int>>;
