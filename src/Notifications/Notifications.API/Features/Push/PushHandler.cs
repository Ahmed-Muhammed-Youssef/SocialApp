using Npgsql;

namespace Notifications.API.Features.Push;

/// <summary>
/// Stores a notification and one delivery per recipient per enabled channel in a single transaction.
/// Nothing is sent here: once this returns, the delivery workers take over.
/// </summary>
public sealed class PushHandler(
    NotificationsDbContext db,
    ChannelRegistry channels,
    DeliverySignals signals,
    TimeProvider timeProvider,
    ILogger<PushHandler> logger)
{
    /// <returns><see langword="false"/> if a notification with the same EventId was already accepted.</returns>
    public async Task<bool> HandleAsync(PushRequest request, CancellationToken cancellationToken)
    {
        DateTimeOffset now = timeProvider.GetUtcNow();
        IReadOnlyList<string> enabledChannels = channels.EnabledChannels;

        // Npgsql only writes UTC values to timestamptz columns.
        Notification notification = new()
        {
            EventId = request.EventId,
            Title = request.Title,
            Body = request.Body,
            Data = request.Data?.ToJsonString(),
            CreatedAt = (request.CreatedAt ?? now).ToUniversalTime(),
            ExpiresAt = request.ExpiresAt?.ToUniversalTime(),
            ReceivedAt = now,
        };

        string[] recipientIds = [.. request.Recipients.Distinct(StringComparer.Ordinal)];

        foreach (string recipientId in recipientIds)
        {
            foreach (string channel in enabledChannels)
            {
                notification.Deliveries.Add(new Delivery
                {
                    EventId = request.EventId,
                    RecipientId = recipientId,
                    Channel = channel,
                    Status = DeliveryStatus.Pending,
                    NextAttemptAt = now,
                    CreatedAt = now,
                });
            }
        }

        // One transaction: the counter row locks taken below are held until it commits or rolls back.
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        db.Notifications.Add(notification);

        // Insert the notification first, so a duplicate fails here, before any counter is locked.
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            logger.DuplicatePush(request.EventId);
            return false;
        }

        Dictionary<string, long> sequences = await NextSequencesAsync(recipientIds, cancellationToken);
        foreach (string recipientId in recipientIds)
        {
            db.InboxItems.Add(new InboxItem
            {
                RecipientId = recipientId,
                Seq = sequences[recipientId],
                EventId = request.EventId,
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        foreach (string channel in enabledChannels)
        {
            signals.Notify(channel);
        }

        return true;
    }

    /// <summary>
    /// Takes the next inbox sequence number for each recipient, creating counters for first-time recipients.
    /// Must run inside the push transaction: each counter row stays locked until commit.
    /// </summary>
    private async Task<Dictionary<string, long>> NextSequencesAsync(string[] recipientIds, CancellationToken cancellationToken)
    {
        // TODO(human): one SQL statement that increments (or creates at 1) inbox_counters.last_seq for every
        // id in recipientIds, locking rows in a consistent order, and returns each recipient's new value.
        // Run it with db.Database.SqlQuery<RecipientSeq>($"...") and turn the rows into a dictionary.
        throw new NotImplementedException();
    }

    private sealed record RecipientSeq(string RecipientId, long Seq);
}
