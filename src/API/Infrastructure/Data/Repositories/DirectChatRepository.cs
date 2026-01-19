namespace Infrastructure.Data.Repositories;

public class DirectChatRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<DirectChat>(dataContext), IDirectChatRepository
{
    /// <inheritdoc/>
    public async Task<DirectChat> GetOrAddAsync(int user1Id, int user2Id, CancellationToken cancellationToken = default)
    {
        (user1Id, user2Id) = DirectChat.OrderIds(user1Id, user2Id);

        var chat = await dataContext.DirectChats
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.User1Id == user1Id && c.User2Id == user2Id, cancellationToken: cancellationToken);

        if (chat is null)
        {
            chat = new DirectChat(user1Id, user2Id);
            dataContext.DirectChats.Add(chat);
        }

        return chat;
    }

    public async Task<PagedList<DirectChatDTO>> GetChatsDtoAsync(int userId, PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        IQueryable<DirectChat> query = dataContext.DirectChats
        .Where(c => (c.User1Id == userId || c.User2Id == userId) &&
                    dataContext.Messages.Any(m => m.ChatId == c.Id))
        .AsNoTracking();

        int count = await query.CountAsync(cancellationToken);

        List<DirectChatDTO> chats = await query
        .OrderByDescending(c => dataContext.Messages
            .Where(m => m.ChatId == c.Id)
            .Max(m => m.SentDate))
        .Skip(paginationParams.SkipValue())
        .Take(paginationParams.ItemsPerPage)
        .Select(c => new
        {
            Chat = c,
            PeerId = c.User1Id == userId ? c.User2Id : c.User1Id,
            PeerUser = dataContext.ApplicationUsers
                .Where(u => u.Id == (c.User1Id == userId ? c.User2Id : c.User1Id))
                .Select(u => new { u.FirstName, u.LastName })
                .FirstOrDefault(),
            LastMessage = dataContext.Messages
                .Where(m => m.ChatId == c.Id)
                .OrderByDescending(m => m.SentDate)
                .Select(MessageMappings.ToDtoExpression)
                .FirstOrDefault()
        })
        .Select(x => new DirectChatDTO
        (
            UserId: x.PeerId,
            UserFirstName: x.PeerUser != null ? x.PeerUser.FirstName : "",
            UserLastName: x.PeerUser != null ? x.PeerUser.LastName : "",
            LastMessage: x.LastMessage!
        ))
        .ToListAsync(cancellationToken);
        return new PagedList<DirectChatDTO>(chats, count, paginationParams.PageNumber, paginationParams.ItemsPerPage);
    }
    public async Task<List<MessageDTO>> GetMessagesDTOThreadAsync(int user1Id, int user2Id, CancellationToken cancellationToken = default)
    {

        (int firstId, int secondId) = DirectChat.OrderIds(user1Id, user2Id);

        List<MessageDTO> messages = await dataContext.DirectChats.Where(c => c.User1Id == firstId && c.User2Id == secondId)
            .SelectMany(c => c.Messages)
            .Select(MessageMappings.ToDtoExpression)
            .OrderBy(m => m.SentDate)
            .ToListAsync(cancellationToken: cancellationToken) ?? [];

        return messages;
    }
}
