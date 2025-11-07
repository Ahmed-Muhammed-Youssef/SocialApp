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
    public Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int issuerId, int theOtherUserId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //var query = dataContext.Messages
        //    .AsNoTracking()
        //    .Where(m => m.SenderId == issuerId && m.RecipientId == theOtherUserId && !m.SenderDeleted || m.SenderId == theOtherUserId && m.RecipientId == issuerId && !m.RecipientDeleted)
        //    .OrderBy(m => m.SentDate);

        //// update unread messages state to read
        //var unreadMessages = query.Where(m => m.ReadDate == null && m.RecipientId == issuerId);
        //if (unreadMessages.Any())
        //{
        //    foreach (var message in unreadMessages)
        //    {
        //        message.ReadDate = DateTime.UtcNow;
        //    }
        //}

        //return await query.Select(m => MessageMappings.ToDto(m)).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<SimplifiedUserDTO>> GetInboxAsync(int userId)
    {
        var friends = await dataContext.Friends.Where(fr => fr.UserId == userId).Include(f => f.FriendUser).Select(f => new SimplifiedUserDTO
        {
            Id = f.FriendId,
            FirstName = f.FriendUser!.FirstName,
            LastName = f.FriendUser.LastName,
            ProfilePictureUrl = null
        }).ToListAsync();

        return friends;
    }
}
