
namespace Application.Features.DirectChats.List;

public record GetChatsQuery(PaginationParams PaginationParams) : IQuery<Result<PagedList<DirectChatDTO>>>;