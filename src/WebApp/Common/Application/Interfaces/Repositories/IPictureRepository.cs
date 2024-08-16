using Application.DTOs.Picture;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPictureRepository
    {
        public void DeletePicture(Picture picture);
        public Task<Picture> AddPictureAsync(Picture picture);
        public Task<IEnumerable<Picture>> GetUserPictureAsync(int id);
        public Task<IEnumerable<PictureDTO>> GetUserPictureDTOsAsync(int id);
        public void UpdatePicture(Picture picture);
    }
}
