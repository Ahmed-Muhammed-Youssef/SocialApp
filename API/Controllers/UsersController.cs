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
using AutoMapper;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        // GET: api/Users
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await userRepository.GetUsersDTOAsync();
            return Ok(users);
        }

        [HttpGet("info/id/{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await userRepository.GetUserDTOByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("info/username/{username}")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            var user = await userRepository.GetUserDTOByUsernameAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("update")]
        public async Task<ActionResult<UpdatedUserDTO>> PutUser(UpdatedUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(userDTO);
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var appUser = await userRepository.GetUserByEmailAsync(email);

            if (appUser == null)
            {
                return BadRequest(userDTO);
            }
            mapper.Map(userDTO, appUser);
            userRepository.Update(appUser);

            if(await userRepository.SaveAllAsync())
            {
                return Ok(userDTO);
            }
            return BadRequest(userDTO);
        }

        // Deprecated manual object mapping
        /*private UserDTO AppUsertoDTO(AppUser user) =>
            new UserDTO()
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Sex = user.Sex,
                Interest = user.Interest,
                // Age = user.GetAge(),
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
        };*/
    }
}
