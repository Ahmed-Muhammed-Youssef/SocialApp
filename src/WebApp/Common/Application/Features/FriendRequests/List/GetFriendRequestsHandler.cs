namespace Application.Features.FriendRequests.List;

public class GetFriendRequestsHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IQueryHandler<GetFriendRequstsQuery, Result<List<UserDTO>>>
{
    public async ValueTask<Result<List<UserDTO>>> Handle(GetFriendRequstsQuery query, CancellationToken cancellationToken)
    {
        List<UserDTO> friendRequests = await unitOfWork.FriendRequestRepository.
            GetRecievedFriendRequestsAsync(currentUserService.GetPublicId());

        return Result<List<UserDTO>>.Success(friendRequests);
    }
}
