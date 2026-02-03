namespace Application.Features.Users.Posts.Create;

public record CreatePostCommand(string Content) : ICommand<Result<ulong>>;
