namespace Application.Features.FriendRequests.Accept;

public class AcceptFriendRequestHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<AcceptFriendRequestCommand, Result<FriendCreatedResponse>>
{
    public async ValueTask<Result<FriendCreatedResponse>> Handle(AcceptFriendRequestCommand command, CancellationToken cancellationToken)
    {
        try
        {
            int userId = currentUserService.GetPublicId();

            FriendRequest? friendRequest = await unitOfWork.FriendRequestRepository.GetByIdAsync(command.Id, cancellationToken);

            if (friendRequest == null)
            {
                return Result<FriendCreatedResponse>.NotFound();
            }

            friendRequest.Accept(userId);

            unitOfWork.FriendRequestRepository.Update(friendRequest);

            var friend = Friend.CreateFromAcceptedRequest(friendRequest.RequesterId, friendRequest.RequestedId);

            unitOfWork.FriendRepository.Add(friend);

            await unitOfWork.CommitAsync(cancellationToken);

            return Result<FriendCreatedResponse>.Created(new FriendCreatedResponse(command.Id, friend.Created));
        }
        catch (DomainException e)
        {
            return Result<FriendCreatedResponse>.Error(e.Message);
        }
    }
}
