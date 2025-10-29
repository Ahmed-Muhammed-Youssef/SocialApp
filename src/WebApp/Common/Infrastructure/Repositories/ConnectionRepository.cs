using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ConnectionRepository(DataContext dataContext) : RepositoryBase<Connection>(dataContext), IConnectionRepository
{
}
