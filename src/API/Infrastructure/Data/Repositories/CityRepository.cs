namespace Infrastructure.Data.Repositories;

public class CityRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<City>(dataContext), ICityRepository
{
}
