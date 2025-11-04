using Domain.Entities;
using Shared.RepositoryBase;

namespace Application.Common.Interfaces.Repositories;

public interface IGroupRepository : IRepositoryBase<Group>
{
    Task<Group?> GetGroupByName(string groupName, CancellationToken cancellationToken = default);
    Task<Group?> GetGroupByConnectionId(string connectionId, CancellationToken cancellationToken = default);
}
