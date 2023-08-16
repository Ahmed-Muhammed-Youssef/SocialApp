using API.DTOs;
using API.Entities;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUsersRepository
    {
        public Task<Picture> AddPictureAsync(Picture picture);
        public Task<char> GetUserInterest(int userId);
        public void Update(AppUser appUser);
        public void UpdatePicture(Picture picture);
        public Task<PagedList<UserDTO>> GetUsersDTOAsync(string username, UserParams userParams, List<int> ForbiddenIds);
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<UserDTO> GetUserDTOByIdAsync(int id);
        public Task<UserDTO> GetUserDTOByUsernameAsync(string username);
        public Task<AppUser> GetUserByUsernameAsync(string username);
        public Task<UserDTO> GetUserDTOByEmailAsync(string email);
        public void DeleteUser(AppUser user);
        public Task<bool> UserExistsAsync(int id);
        public Task<IEnumerable<PictureDTO>> GetUserPictureDTOsAsync(int id);
        public Task<IEnumerable<Picture>> GetUserPictureAsync(int id);
        public Task<AppUser> GetUserByEmailAsync(string email);
        public void DeletePicture(Picture picture);
        public Task<Picture> GetProfilePictureAsync(int userId);
    }
}
