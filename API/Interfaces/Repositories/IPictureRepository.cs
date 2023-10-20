using API.DTOs;
using API.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces.Repositories
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
