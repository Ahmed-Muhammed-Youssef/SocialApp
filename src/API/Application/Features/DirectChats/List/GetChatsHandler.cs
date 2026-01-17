namespace Application.Features.DirectChats.List;

public class GetChatsHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork) : IQueryHandler<GetChatsQuery, Result<PagedList<DirectChatDTO>>>
{
    public async ValueTask<Result<PagedList<DirectChatDTO>>> Handle(GetChatsQuery query, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetPublicId();
        PagedList<DirectChatDTO> pagedChats = await unitOfWork.DirectChatRepository.GetChatsDtoAsync(userId, query.PaginationParams, cancellationToken);

        return Result<PagedList<DirectChatDTO>>.Success(pagedChats);
    }
}
