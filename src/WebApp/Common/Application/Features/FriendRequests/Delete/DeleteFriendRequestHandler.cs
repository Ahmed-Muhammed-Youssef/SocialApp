using Application.Common.Interfaces;
using Domain.Entities;
using Mediator;
using Shared.Results;

namespace Application.Features.FriendRequests.Delete;

public class DeleteFriendRequestHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<DeleteFriendRequestCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(DeleteFriendRequestCommand command, CancellationToken cancellationToken)
    {
        var senderId = currentUserService.GetPublicId();

        // check if the api issuer is the freind request sender first.

        FriendRequest? fr = await unitOfWork.FriendRequestRepository.GetFriendRequestAsync(senderId, command.UserId);
        
        // check if the api issuer is the freind requested user
        fr ??= await unitOfWork.FriendRequestRepository.GetFriendRequestAsync(command.UserId, senderId);

        if (fr is null)
        {
            return Result<object?>.Error("You didn't sent a friend request.");
        }

        unitOfWork.FriendRequestRepository.Delete(fr);

        await unitOfWork.SaveChangesAsync();

        return Result<object?>.NoContent();
    }
}
