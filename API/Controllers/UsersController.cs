using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using API.DTOs;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // GET: api/Users
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await userRepository.GetUsersAsync();
            return Ok(users.ToList().ConvertAll(AppUsertoDTO));
        }

        // GET: api/Users/5
        [HttpGet("info/id/{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var appUser = await userRepository.GetUserByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }
            var user = AppUsertoDTO(appUser);
            return Ok(user);
        }
        [HttpGet("info/username/{username}")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            var appUser = await userRepository.GetUserByUsernameAsync(username);

            if (appUser == null)
            {
                return NotFound();
            }
            var user = AppUsertoDTO(appUser);
            return Ok(user);
        }
        private UserDTO AppUsertoDTO(AppUser user) =>
            new UserDTO()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Sex = user.Sex,
                Interest = user.Interest,
                DateOfBirth = user.DateOfBirth,
                LastActive = user.LastActive,
                Created = user.Created,
                Bio = user.Bio,
                Country = user.Country,
                City = user.City,
                Photos = user.Photos.ToList().ConvertAll(PhotoToDTO)
            };
        private PhotoSentDTO PhotoToDTO(Photo photo) => new PhotoSentDTO()
        {
            Id = photo.Id,
            Url = photo.Url,
            Order = photo.Order
        };
    }
}
