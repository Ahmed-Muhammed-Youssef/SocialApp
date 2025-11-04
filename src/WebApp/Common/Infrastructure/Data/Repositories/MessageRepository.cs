using Shared.RepositoryBase;

namespace Infrastructure.Data.Repositories;

public class MessageRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Message>(dataContext), IMessageRepository
{
    public void DeleteMessage(Message message, int issuerId)
    {

        // Delete the message when it's deleted from both sender and recipient
        if (message.SenderId == issuerId)
        {
            if (message.SenderDeleted == false)
            {
                message.SenderDeleted = true;
            }
        }
        else if (message.RecipientId == issuerId)
        {
            if (message.RecipientDeleted == false)
            {
                message.RecipientDeleted = true;
            }
        }
        if (message.RecipientDeleted && message.SenderDeleted)
        {
            dataContext.Messages.Remove(message);
        }
        else
        {
            dataContext.Messages.Update(message);
        }
    }

    public async Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int issuerId, int theOtherUserId, CancellationToken cancellationToken = default)
    {
        var query = dataContext.Messages
            .AsNoTracking()
            .Where(m => m.SenderId == issuerId && m.RecipientId == theOtherUserId && !m.SenderDeleted || m.SenderId == theOtherUserId && m.RecipientId == issuerId && !m.RecipientDeleted)
            .OrderBy(m => m.SentDate);

        // update unread messages state to read
        var unreadMessages = query.Where(m => m.ReadDate == null && m.RecipientId == issuerId);
        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.ReadDate = DateTime.UtcNow;
            }
        }

        return await query.Select(m => MessageMappings.ToDto(m)).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<SimplifiedUserDTO>> GetInboxAsync(int userId)
    {
        var friends = await dataContext.Friends.Where(fr => fr.UserId == userId).Include(f => f.FriendUser).Select(f => new SimplifiedUserDTO
        {
            Id = f.FriendId,
            FirstName = f.FriendUser!.FirstName,
            LastName = f.FriendUser.LastName,
            ProfilePictureUrl =f.FriendUser.ProfilePictureUrl
        }).ToListAsync();

        return friends;
    }
}
