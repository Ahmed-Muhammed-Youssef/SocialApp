namespace API.Features.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserPicturesController(IMediator mediator) : ControllerBase
{
    // POST: api/userpictures
    [HttpPost]
    public async Task<ActionResult<PictureDTO>> Create(IFormFile file, CancellationToken cancellationToken)
    {
        Result<int> result = await mediator.Send(new CreateUserPictureCommand(file), cancellationToken);

        if (result.IsSuccess)
        {
            return Created();
        }
        else if (result.Status == ResultStatus.Unauthorized)
        {
            return Unauthorized(result.Errors);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    // DELETE: api/userpictures/{pictureId}
    [HttpDelete("{pictureId}")]
    public async Task<ActionResult<PictureDTO>> Delete(int pictureId, CancellationToken cancellationToken)
    {
        Result<object?> result = await mediator.Send(new DeleteUserPictureCommand(pictureId), cancellationToken);

        if(result.IsSuccess)
        {
            return NoContent();
        }
        else if(result.Status == ResultStatus.NotFound)
        {
            return NotFound(result.Errors);
        }
        else if(result.Status == ResultStatus.Unauthorized)
        {
            return Unauthorized(result.Errors);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    // GET: api/userpictures
    [HttpGet]
    public async Task<ActionResult<PictureDTO>> Get(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserPicturesQuery(), cancellationToken);

        if(result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }
}
