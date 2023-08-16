using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PicturesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPictureService _pictureService;
        private readonly IMapper _mapper;

        public PicturesController(IUnitOfWork unitOfWork, IPictureService pictureService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _pictureService = pictureService;
            _mapper = mapper;
        }
        // POST: api/pictures/upload
        [HttpPost("upload")]
        public async Task<ActionResult<PictureDTO>> UploadPicture(IFormFile file)
        {
            var user = await _unitOfWork.UsersRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _pictureService.AddPictureAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var photo = new Picture
            {
                AppUserId = user.Id,
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            photo = await _unitOfWork.UsersRepository.AddPictureAsync(photo);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtAction(nameof(UsersController.GetUser), new { username = user.UserName }, _mapper.Map<PictureDTO>(photo));
            }
            return BadRequest();
        }

        // DELETE: api/pictures/delete/{pictureId}
        [HttpDelete("delete/{pictureId}")]
        public async Task<ActionResult<PictureDTO>> DeletePhoto(int pictureId)
        {
            var user = await _unitOfWork.UsersRepository.GetUserByUsernameAsync(User.GetUsername());
            var pictures = await _unitOfWork.UsersRepository.GetUserPictureAsync(user.Id);
            var picture = pictures.FirstOrDefault(p => p.Id == pictureId);
            if (picture == null)
            {
                return BadRequest($"{pictureId} doesn't exist.");
            }
            if (picture.AppUserId != user.Id)
            {
                return Unauthorized();
            }
            if (picture.Order == 0)
            {
                return BadRequest("you can't delete your profile picture.");
            }
            var result = await _pictureService.DeletePictureAsync(picture.PublicId);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            _unitOfWork.UsersRepository.DeletePicture(picture);

            // reorder the photos
            var picturesList = pictures.ToList();
            for (int i = picture.Order + 1; i < picturesList.Count(); i++)
            {
                picturesList[i].Order--;
                _unitOfWork.UsersRepository.UpdatePicture(picturesList[i]);
            }
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("failed to delete the image from the server.");
        }

        // GET: api/pictures/all
        [HttpGet("all")]
        public async Task<ActionResult<PictureDTO>> GetPictures()
        {
            var pictures = await _unitOfWork.UsersRepository.GetUserPictureDTOsAsync(User.GetId()); //the output is ordered
            return Ok(pictures);
        }

        // GET: api/pictures/reorder
        [HttpPut("pictures/reorder")]
        public async Task<ActionResult<PictureDTO>> ReorderPictures(IEnumerable<PictureDTO> pictureDTOs)
        {
            var user = await _unitOfWork.UsersRepository.GetUserByUsernameAsync(User.GetUsername());
            var pictures = await _unitOfWork.UsersRepository.GetUserPictureAsync(user.Id); //the output is ordered
            if (pictureDTOs.Count() != pictures.Count())
            {
                return BadRequest(pictureDTOs);
            }
            var listOfDTOs = pictureDTOs.OrderBy(p => p.Order).ToList();
            // checks if the incoming list has invalid order values
            for (int i = 0; i < listOfDTOs.Count(); i++)
            {
                if (listOfDTOs[i].Order != i)
                {
                    return BadRequest(pictureDTOs);
                }
            }
            foreach (var picture in pictures)
            {
                if (picture.Id == listOfDTOs[picture.Order].Id)
                {
                    if (picture.Url != listOfDTOs[picture.Order].Url)
                    {
                        return BadRequest(pictureDTOs);
                    }
                    continue; // the current picture order still the same.
                }
                // the following executes if the current picture order is changed.
                var pictureDTO = listOfDTOs.FirstOrDefault(p => p.Id == picture.Id);
                // checks if there is any data changed rather than the order.
                if (pictureDTO.Url != picture.Url)
                {
                    return BadRequest(pictureDTOs);
                }
                picture.Order = pictureDTO.Order;
                _unitOfWork.UsersRepository.UpdatePicture(picture);
            }
            if (await _unitOfWork.Complete())
            {
                return Ok(pictureDTOs);
            }
            return BadRequest("nothing has changed.");
        }
    }
}
