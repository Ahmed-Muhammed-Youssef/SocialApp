namespace Application.Features.FriendRequests.Delete;

public class DeleteFriendRequestHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<DeleteFriendRequestCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(DeleteFriendRequestCommand command, CancellationToken cancellationToken)
    {
        var senderId = currentUserService.GetPublicId();

        FriendRequest? fr = await unitOfWork.FriendRequestRepository.GetByIdAsync(command.Id, cancellationToken);
        
        if (fr is null || fr.RequesterId != senderId || fr.Status != RequestStatus.Pending)
        {
            return Result<object?>.Error("You didn't sent a friend request.");
        }

        unitOfWork.FriendRequestRepository.Delete(fr);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result<object?>.NoContent();
    }
}
