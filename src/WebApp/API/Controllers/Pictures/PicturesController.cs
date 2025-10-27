namespace API.Controllers.Pictures;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PicturesController(IMediator mediator) : ControllerBase
{
    // POST: api/pictures
    [HttpPost]
    public async Task<ActionResult<PictureDTO>> Create(IFormFile file)
    {
        Result<int> result = await mediator.Send(new CreatePictureCommand(file));

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

    // DELETE: api/pictures/{pictureId}
    [HttpDelete("{pictureId}")]
    public async Task<ActionResult<PictureDTO>> Delete(int pictureId)
    {
        Result<object?> result = await mediator.Send(new DeletePictureCommand(pictureId));

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
