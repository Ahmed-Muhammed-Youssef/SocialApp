namespace Infrastructure.Data.Repositories;

public class PictureRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Picture>(dataContext), IPictureRepository
{
    public Task<List<Picture>> GetUserPictureAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<PictureDTO>> GetUserPictureDTOsAsync(int id)
    {
        throw new NotImplementedException();
    }
}
