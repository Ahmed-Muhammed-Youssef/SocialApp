using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        // POST: api/pictures
        [HttpPost]
        public async Task<ActionResult<PictureDTO>> UploadPicture(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            var result = await _pictureService.AddPictureAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var picture = new Picture
            {
                AppUserId = user.Id,
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            picture = await _unitOfWork.PictureRepository.AddPictureAsync(picture);

            if (await _unitOfWork.Complete())
            {
                return Ok(_mapper.Map<PictureDTO>(picture));
            }
            return BadRequest();
        }

        // POST: api/pictures/profilepicture
        [HttpPost("profilepicture/{pictureId}")]
        public async Task<ActionResult<PictureDTO>> SetProfilePicture(int pictureId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            var pictures = await _unitOfWork.PictureRepository.GetUserPictureAsync(user.Id);
            var picture = pictures.FirstOrDefault(p => p.Id == pictureId);
            if (picture == null)
            {
                return BadRequest($"{pictureId} doesn't exist.");
            }
            if (picture.AppUserId != user.Id)
            {
                return Unauthorized();
            }
            user.ProfilePictureUrl = picture.Url;
            _unitOfWork.UserRepository.Update(user);
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("failed to set the profile picture.");
        }
        // DELETE: api/pictures/{pictureId}
        [HttpDelete("{pictureId}")]
        public async Task<ActionResult<PictureDTO>> DeletePhoto(int pictureId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            var pictures = await _unitOfWork.PictureRepository.GetUserPictureAsync(user.Id);
            var picture = pictures.FirstOrDefault(p => p.Id == pictureId);
            if (picture == null)
            {
                return BadRequest($"{pictureId} doesn't exist.");
            }
            if (picture.AppUserId != user.Id)
            {
                return Unauthorized();
            }
            var result = await _pictureService.DeletePictureAsync(picture.PublicId);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            _unitOfWork.PictureRepository.DeletePicture(picture);

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
            var pictures = await _unitOfWork.PictureRepository.GetUserPictureDTOsAsync(User.GetId()); //the output is ordered
            return Ok(pictures);
        }
    }
}
