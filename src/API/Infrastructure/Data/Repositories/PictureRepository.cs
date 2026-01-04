namespace Infrastructure.Data.Repositories;

public class PictureRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Picture>(dataContext), IPictureRepository
{
    public async Task<List<Picture>> GetUserPictureAsync(int id)
    {
        List<Picture> pictures = await dataContext.UserPictures
        .Where(up => up.UserId == id)
        .Join(
            dataContext.Pictures,
            up => up.PictureId,
            p => p.Id,
            (up, p) => p
        )
        .OrderByDescending(p => p.Created)
        .ToListAsync();

        return pictures;
    }

    public async Task<List<PictureDTO>> GetUserPictureDTOsAsync(int id)
    {
        var pictures = await dataContext.UserPictures
        .Where(up => up.UserId == id)
        .Join(
            dataContext.Pictures,
            up => up.PictureId,
            p => p.Id,
            (up, p) => p
        )
        .OrderByDescending(p => p.Created)
        .Select(p => new PictureDTO(p.Id, p.Url))
        .ToListAsync();

        return pictures;
    }
}
