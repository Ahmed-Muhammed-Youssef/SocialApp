using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.DTOs;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using API.Extensions;
using API.Helpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.photoService = photoService;
        }

        // GET: api/Users
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] UserParams userParams)
        {
            if(string.IsNullOrEmpty(userParams.Sex))
            {
                var interest = await unitOfWork.UserRepository.GetUserInterest(User.GetId());
                userParams.Sex = interest.ToString();
            }
            var forbiddenIds = await unitOfWork.LikesRepository.GetLikedUsersIdAsync(User.GetId());
            var users = await unitOfWork.UserRepository.GetUsersDTOAsync(User.GetUsername() ,userParams, forbiddenIds.ToList());
            var newPaginationHeader = new PaginationHeader(users.CurrentPage, users.ItemsPerPage, users.TotalCount, users.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader);
            return Ok(users);
        }

        [HttpGet("info/id/{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await unitOfWork.UserRepository.GetUserDTOByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("info/username/{username}")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            var user = await unitOfWork.UserRepository.GetUserDTOByUsernameAsync(username);

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
            var appUser = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (appUser == null)
            {
                return BadRequest(userDTO);
            }
            mapper.Map(userDTO, appUser);
            unitOfWork.UserRepository.Update(appUser);

            if(await unitOfWork.Complete())
            {
                return Ok(userDTO);
            }
            return BadRequest(userDTO);
        }
        [HttpPost("photo/upload")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
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

            photo = await unitOfWork.UserRepository.AddPhotoAsync(photo);

            if(await unitOfWork.Complete())
            {
                return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, mapper.Map<PhotoDTO>(photo));
            }
            return BadRequest();
        }
        [HttpDelete("photo/delete/{photoId}")]
        public async Task<ActionResult<PhotoDTO>> DeletePhoto(int photoId)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var photos = await unitOfWork.UserRepository.GetUserPhotoAsync(user.Id);
            var photo = photos.FirstOrDefault(p => p.Id == photoId);
            if(photo == null)
            {
                return BadRequest($"{photoId} doesn't exist.");
            }
            if(photo.AppUserId != user.Id)
            {
                return Unauthorized();
            }
            if(photo.Order == 0)
            {
                return BadRequest("you can't delete your profile picture.");
            }
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            unitOfWork.UserRepository.DeletePhoto(photo);

            // reorder the photos
            var photosList = photos.ToList();
            for (int i = photo.Order + 1; i < photosList.Count(); i++)
            {
                photosList[i].Order--;
                unitOfWork.UserRepository.UpdatePhoto(photosList[i]);
            }
            if (await unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("failed to delete the image from the server.");
        }
        [HttpGet("photos/all")]
        public async Task<ActionResult<PhotoDTO>> GetPhotos()
        {
            var photos = await unitOfWork.UserRepository.GetUserPhotoDTOsAsync(User.GetId()); //the output is ordered
            return Ok(photos);
        }
        [HttpPut("photos/reorder")]
        public async Task<ActionResult<PhotoDTO>> ReorderPhotos(IEnumerable<PhotoDTO> photoDTOs)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var photos = await unitOfWork.UserRepository.GetUserPhotoAsync(user.Id); //the output is ordered
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
                unitOfWork.UserRepository.UpdatePhoto(photo);
            }
            if(await unitOfWork.Complete())
            {
                return Ok(photoDTOs);
            }
            return BadRequest("nothing has changed.");
        }
    }
}
