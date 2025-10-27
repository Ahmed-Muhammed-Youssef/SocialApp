using Application.Features.Pictures;
using Application.Features.Pictures.List;

namespace API.Controllers.Pictures;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PicturesController(IUnitOfWork _unitOfWork, IPictureService _pictureService, IMapper _mapper, IMediator mediator) : ControllerBase
{
    // POST: api/pictures
    [HttpPost]
    public async Task<ActionResult<PictureDTO>> Create(IFormFile file)
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
    public async Task<ActionResult<PictureDTO>> Delete(int pictureId)
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

    // GET: api/pictures
    [HttpGet]
    public async Task<ActionResult<PictureDTO>> Get()
    {
        var result = await mediator.Send(new GetPicturesQuery());

        if(result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }
}
