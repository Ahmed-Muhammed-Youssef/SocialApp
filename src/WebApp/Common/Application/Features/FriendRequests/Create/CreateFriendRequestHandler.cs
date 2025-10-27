using Application.Interfaces;
using Application.Interfaces.Services;
using Mediator;
using Shared.Results;

namespace Application.Features.FriendRequests.Create;

public class CreateFriendRequestHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<CreateFriendRequestCommand, Result<int>>
{
    public async ValueTask<Result<int>> Handle(CreateFriendRequestCommand command, CancellationToken cancellationToken)
    {
        // retuns true if the user has become a frined
        var senderId = currentUserService.GetPublicId();
        var targetId = command.UserId;

        if (senderId == targetId)
        {
            return Result<int>.Error("You can't send friend requests to yourself.");
        }

        if (await unitOfWork.FriendRequestRepository.GetFriendRequestAsync(senderId, targetId) != null)
        {
            return Result<int>.Error("You already sent a frient request to this user.");
        }

        if (await unitOfWork.FriendRequestRepository.IsFriend(senderId, targetId) == true)
        {
            return Result<int>.Error("You already are friends.");
        }

        // must pervent user from sending friend request if the target user already sent him a friend request
        bool isFriend = await unitOfWork.FriendRequestRepository.SendFriendRequest(senderId, targetId);

        await unitOfWork.SaveChangesAsync();

        // @todo: must return the friend request id instead of 1 or 0

        return Result<int>.Created(isFriend ? 1 : 0);
    }
}
