using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class GroupRepository(DataContext dataContext) : RepositoryBase<Group>(dataContext), IGroupRepository
{
}
