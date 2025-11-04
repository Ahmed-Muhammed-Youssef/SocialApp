namespace Infrastructure.Data.Repositories;

public class GroupRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Group>(dataContext), IGroupRepository
{
    public async Task<Group?> GetGroupByName(string groupName, CancellationToken cancellationToken = default)
    {
        return await dataContext.Groups
        .Include(g => g.Connections)
        .FirstOrDefaultAsync(g => g.Name == groupName, cancellationToken: cancellationToken);
    }

    public async Task<Group?> GetGroupByConnectionId(string connectionId, CancellationToken cancellationToken = default)
    {
        return await dataContext.Groups
        .AsNoTracking()
        .Include(g => g.Connections)
        .Where(g => g.Connections.Any(c => c.ConnectionId == connectionId))
        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}
