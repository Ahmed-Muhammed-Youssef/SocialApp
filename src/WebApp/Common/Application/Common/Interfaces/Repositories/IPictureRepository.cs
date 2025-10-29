using Application.Features.Pictures;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IPictureRepository : IRepositoryBase<Picture>
{
    public void DeletePicture(Picture picture);
    public Task<Picture> AddPictureAsync(Picture picture);
    public Task<List<Picture>> GetUserPictureAsync(int id);
    public Task<List<PictureDTO>> GetUserPictureDTOsAsync(int id);
    public void UpdatePicture(Picture picture);
}
