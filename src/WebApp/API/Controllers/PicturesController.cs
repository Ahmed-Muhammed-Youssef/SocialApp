namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PicturesController(IUnitOfWork _unitOfWork, IPictureService _pictureService, IMapper _mapper) : ControllerBase
{
    // POST: api/pictures
    [HttpPost]
    public async Task<ActionResult<PictureDTO>> UploadPicture(IFormFile file)
    {
        var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId());

        if(user is null)
        {
            return Unauthorized();
        }

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

        await _unitOfWork.SaveChangesAsync();

        return Ok(_mapper.Map<PictureDTO>(picture));
    }

    // POST: api/pictures/profilepicture
    [HttpPost("profilepicture/{pictureId}")]
    public async Task<ActionResult<PictureDTO>> SetProfilePicture(int pictureId)
    {
        var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId());

        if (user is null)
        {
            return Unauthorized();
        }

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
        _unitOfWork.ApplicationUserRepository.Update(user);

        await _unitOfWork.SaveChangesAsync();

        return Ok();
    }
    // DELETE: api/pictures/{pictureId}
    [HttpDelete("{pictureId}")]
    public async Task<ActionResult<PictureDTO>> DeletePhoto(int pictureId)
    {
        ApplicationUser? user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId());

        if(user == null)
        {
            return Unauthorized();
        }

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

        if(user.ProfilePictureUrl == picture.Url)
        {
            user.ProfilePictureUrl = "";

            _unitOfWork.ApplicationUserRepository.Update(user);
        }

        await _unitOfWork.SaveChangesAsync();
        
        return Ok();
    }

    // GET: api/pictures/all
    [HttpGet("all")]
    public async Task<ActionResult<PictureDTO>> GetPictures()
    {
        var pictures = await _unitOfWork.PictureRepository.GetUserPictureDTOsAsync(User.GetPublicId()); //the output is ordered
        return Ok(pictures);
    }
}
