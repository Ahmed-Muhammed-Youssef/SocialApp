namespace Application.Features.Users.List;

public class GetUsersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IQueryHandler<GetUsersQuery, Result<PagedList<UserDTO>>>
{
    public async ValueTask<Result<PagedList<UserDTO>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        PagedList<UserDTO> users = await unitOfWork.ApplicationUserRepository.GetUsersDTOAsync(currentUserService.GetPublicId(), query.UserParams, cancellationToken);

        return Result<PagedList<UserDTO>>.Success(users);
    }
}
