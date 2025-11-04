using Shared.RepositoryBase;

namespace Infrastructure.Data.Repositories;

public class ConnectionRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Connection>(dataContext), IConnectionRepository
{
}
