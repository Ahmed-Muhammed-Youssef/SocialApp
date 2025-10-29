using Application.Common.Interfaces;
using Domain.Entities;
using Mediator;
using Shared.Results;

namespace Application.Features.Posts.Create;

public class CreatePostHandler(IUnitOfWork _unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<CreatePostCommand, Result<ulong>>
{
    public async ValueTask<Result<ulong>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        Post post = new()
        {
            Content = command.Content,
            DatePosted = DateTime.UtcNow,
            UserId = currentUserService.GetPublicId()
        };

        await _unitOfWork.PostRepository.AddAsync(post);

        await _unitOfWork.SaveChangesAsync();

        return Result<ulong>.Created(post.Id);
    }
}
