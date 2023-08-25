using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
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
                return Ok(_mapper.Map<PictureDTO>(photo));
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
            var result = await _pictureService.DeletePictureAsync(picture.PublicId);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            _unitOfWork.UsersRepository.DeletePicture(picture);

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
    }
}
