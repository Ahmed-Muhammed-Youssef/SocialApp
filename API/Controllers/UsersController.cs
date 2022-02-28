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
using Microsoft.AspNetCore.Http;
using API.Extensions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.photoService = photoService;
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
            var appUser = await userRepository.GetUserByUsernameAsync(User.GetUsername());

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
        [HttpPost("photo/upload")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await photoService.AddPhotoAsync(file);
            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var photo = new Photo
            {
                AppUserId = user.Id,
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            photo = await userRepository.AddPhotoAsync(photo);

            if(await userRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, mapper.Map<PhotoDTO>(photo));
            }
            return BadRequest();
        }
        [HttpPost("photo/delete/{photoId}")]
        public async Task<ActionResult<PhotoDTO>> DeletePhoto(int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photos = await userRepository.GetUserPhotoAsync(user.Id);
            var photo = photos.FirstOrDefault(p => p.Id == photoId);
            if(photo == null)
            {
                return BadRequest($"{photoId} doesn't exist.");
            }
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            userRepository.DeletePhoto(photo);
            if (await userRepository.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("failed to delete the image from the server.");
        }
        [HttpPut("photos/reorder")]
        public async Task<ActionResult<PhotoDTO>> ReorderPhotos(IEnumerable<PhotoDTO> photoDTOs)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photos = await userRepository.GetUserPhotoAsync(user.Id); //the output is ordered
            if(photoDTOs.Count() != photos.Count())
            {
                return BadRequest(photoDTOs);
            }
            var listOfDTOs = photoDTOs.OrderBy(p => p.Order).ToList();
            // checks if the incoming list has invalid order values
            for (int i = 0; i < listOfDTOs.Count(); i++)
            {
                if(listOfDTOs[i].Order != i)
                {
                    return BadRequest(photoDTOs);
                }
            }
            foreach (var photo in photos)
            {
                if(photo.Id == listOfDTOs[photo.Order].Id)
                {
                    if(photo.Url != listOfDTOs[photo.Order].Url)
                    {
                        return BadRequest(photoDTOs);
                    }
                    continue; // the current photo order still the same.
                }
                // the following executes if the current photo order is changed.
                var photoDTO = listOfDTOs.FirstOrDefault(p => p.Id == photo.Id);
                // checks if there is any data changed rather than the order.
                if(photoDTO.Url != photo.Url)
                {
                    return BadRequest(photoDTOs);
                }
                photo.Order = photoDTO.Order;
                userRepository.UpdatePhoto(photo);
            }
            if(await userRepository.SaveAllAsync())
            {
                return Ok(photoDTOs);
            }
            return BadRequest("nothing has changed.");
        }
    }
}
