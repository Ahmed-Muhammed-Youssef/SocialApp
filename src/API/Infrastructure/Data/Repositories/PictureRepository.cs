namespace Infrastructure.Data.Repositories;

public class PictureRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Picture>(dataContext), IPictureRepository
{
    public async Task<List<Picture>> GetUserPictureAsync(int userId, CancellationToken cancellationToken = default)
    {
        List<Picture> pictures = await dataContext.UserPictures
        .Where(up => up.UserId == userId)
        .Join(
            dataContext.Pictures,
            up => up.PictureId,
            p => p.Id,
            (up, p) => p
        )
        .OrderByDescending(p => p.Created)
        .ToListAsync(cancellationToken: cancellationToken);

        return pictures;
    }

    public async Task<PictureDTO?> GetUserPictureDTOAsync(int userId, int pictureId, CancellationToken cancellationToken = default)
    {
        PictureDTO? userPicture = await dataContext.UserPictures
        .Where(up => up.UserId == userId && up.PictureId == pictureId)
        .Join(
            dataContext.Pictures,
            up => up.PictureId,
            p => p.Id,
            (up, p) => p
        )
        .Select(PictureMappings.ToDtoExpression)
        .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return userPicture;
    }

    public async Task<List<PictureDTO>> GetUserPictureDTOsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var pictures = await dataContext.UserPictures
        .Where(up => up.UserId == userId)
        .Join(
            dataContext.Pictures,
            up => up.PictureId,
            p => p.Id,
            (up, p) => p
        )
        .OrderByDescending(p => p.Created)
        .Select(p => new PictureDTO(p.Id, p.Url))
        .ToListAsync(cancellationToken: cancellationToken);

        return pictures;
    }
}
