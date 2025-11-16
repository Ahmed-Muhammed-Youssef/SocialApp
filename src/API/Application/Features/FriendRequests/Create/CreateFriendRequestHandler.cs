namespace Application.Features.FriendRequests.Create;

public class CreateFriendRequestHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<CreateFriendRequestCommand, Result<int>>
{
    public async ValueTask<Result<int>> Handle(CreateFriendRequestCommand command, CancellationToken cancellationToken)
    {
        try
        {
            int senderId = currentUserService.GetPublicId();
            int targetId = command.UserId;

            FriendRequest friendRequest = FriendRequest.Create(senderId, targetId);
            var receivedFr = await unitOfWork.FriendRequestRepository.GetFriendRequestAsync(targetId, senderId);

            if (receivedFr is not null && receivedFr.Status == RequestStatus.Pending)
            {
                return Result<int>.Error("You already have a pending frient request from this user.");
            }

            if (await unitOfWork.FriendRequestRepository.GetFriendRequestAsync(senderId, targetId) != null)
            {
                return Result<int>.Error("You already sent a frient request to this user.");
            }

            if (await unitOfWork.FriendRequestRepository.IsFriend(senderId, targetId) == true)
            {
                return Result<int>.Error("You already are friends.");
            }

            await unitOfWork.FriendRequestRepository.AddAsync(friendRequest, cancellationToken);

            return Result<int>.Created(friendRequest.Id);
        }
        catch (DomainException ex)
        {
            return Result<int>.Error(ex.Message);
        }
    }
}
