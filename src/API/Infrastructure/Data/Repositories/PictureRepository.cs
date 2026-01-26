namespace Infrastructure.Data.Repositories;

public class PictureRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Picture>(dataContext), IPictureRepository
{
}
