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
    public async Task<List<MessageDTO>> GetMessagesDTOThreadAsync(int issuerId, int theOtherUserId, CancellationToken cancellationToken = default)
    {

        (int user1Id, int user2Id) = DirectChat.OrderIds(issuerId, theOtherUserId);

        List<MessageDTO> messages = await dataContext.DirectChats.Where(c => c.User1Id == user1Id && c.User2Id == user2Id)
            .SelectMany(c => c.Messages)
            .Select(MessageMappings.ToDtoExpression)
            .OrderBy(m => m.SentDate)
            .ToListAsync(cancellationToken: cancellationToken) ?? [];
            
        return messages;
    }
}
