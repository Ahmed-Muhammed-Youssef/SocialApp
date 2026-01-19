namespace Application.Common.Interfaces.Repositories;

public interface IPictureRepository : IRepositoryBase<Picture>
{
    Task<List<Picture>> GetUserPictureAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<PictureDTO>> GetUserPictureDTOsAsync(int userId, CancellationToken cancellationToken = default);
    Task<PictureDTO?> GetUserPictureDTOAsync(int userId, int pictureId, CancellationToken cancellationToken = default);
}
