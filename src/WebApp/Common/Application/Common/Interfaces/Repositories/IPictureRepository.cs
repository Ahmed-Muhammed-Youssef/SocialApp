namespace Application.Common.Interfaces.Repositories;

public interface IPictureRepository : IRepositoryBase<Picture>
{
    public Task<List<Picture>> GetUserPictureAsync(int id);
    public Task<List<PictureDTO>> GetUserPictureDTOsAsync(int id);
}
