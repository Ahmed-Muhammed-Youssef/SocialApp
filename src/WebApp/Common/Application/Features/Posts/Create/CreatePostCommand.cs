using Mediator;
using Shared.Results;

namespace Application.Features.Posts.Create;

public record CreatePostCommand(string Content): ICommand<Result<ulong>>;
