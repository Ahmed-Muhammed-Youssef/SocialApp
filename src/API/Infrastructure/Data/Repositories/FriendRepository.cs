namespace Infrastructure.Data.Repositories;

public class FriendRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Friend>(dataContext), IFriendRepository
{
}
