namespace Application.Features.Users.SetProfilePciture;

public record SetProfilePictureCommand(int PictureId) : ICommand<Result<object?>>;
