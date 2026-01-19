using Application.Features.Users.GetUserPictureById;

namespace API.Features.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(LogUserActivity))]
public class UsersController(JsonSerializerOptions jsonSerializerOptions, IMediator mediator) : ControllerBase
{
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
    {
        UserParams userParam = new()
        {
            MinAge = request.MinAge,
            MaxAge = request.MaxAge,
            OrderBy = request.OrderBy,
            RelationFilter = request.RelationFilter,
            PageNumber = request.PageNumber,
            ItemsPerPage = request.ItemsPerPage
        };

        var result = await mediator.Send(new GetUsersQuery(userParam), cancellationToken);

        if (result.IsSuccess)
        {
            var newPaginationHeader = new PaginationHeader(result.Value.CurrentPage, result.Value.ItemsPerPage, result.Value.Count, result.Value.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader, jsonSerializerOptions);
            return Ok(result.Value.Items);
        }
        else
        {
            return BadRequest(result.Errors);
        }

    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetById(int id, CancellationToken cancellationToken)
    {
        Result<UserDTO> result = await mediator.Send(new GetUserQuery(id), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound();
        }
    }

    // PUT: api/users/update
    [HttpPut]
    public async Task<ActionResult<UserDTO>> Update(UpdateUserRequest userDTO, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Result<UserDTO> result = await mediator.Send(new UpdateUserCommand
        {
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Bio = userDTO.Bio,
            CityId = userDTO.CityId
        }, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Route("set-profile-picture/{pictureId}")]
    public async Task<ActionResult> SetProfilePicture(int pictureId, CancellationToken cancellationToken)
    {
        Result<object?> result = await mediator.Send(new SetProfilePictureCommand(pictureId), cancellationToken);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        else if (result.Status == ResultStatus.NotFound)
        {
            return NotFound(result.Errors);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpGet("{userId}/posts")]
    public async Task<ActionResult<List<PostDTO>>> GetUserPostsAsync(int userId, CancellationToken cancellationToken)
    {
        Result<List<PostDTO>> result = await mediator.Send(new GetUserPostsQuery(userId), cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    // POST: api/users/userpictures
    [HttpPost("user-pictures")]
    public async Task<ActionResult> CreateUserPicture(IFormFile file, CancellationToken cancellationToken)
    {
        Result<int> result = await mediator.Send(new CreateUserPictureCommand(file), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(new { pictureId = result.Value });
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

    // DELETE: api/users/user-pictures/{pictureId}
    [HttpDelete("user-pictures/{pictureId}")]
    public async Task<ActionResult<PictureDTO>> DeleteUserPicture(int pictureId, CancellationToken cancellationToken)
    {
        Result<object?> result = await mediator.Send(new DeleteUserPictureCommand(pictureId), cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }
        else if (result.Status == ResultStatus.NotFound)
        {
            return NotFound(result.Errors);
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

    // GET: api/users/user-pictures
    [HttpGet("user-pictures")]
    public async Task<ActionResult<PictureDTO>> GetUserPictures(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserPicturesQuery(), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }

    // GET: api/users/user-pictures/{pictureId}
    [HttpGet("user-pictures/{pictureId}")]
    public async Task<ActionResult<PictureDTO>> GetUserPictureById(int pictureId, CancellationToken cancellationToken)
    {
        Result<PictureDTO> result = await mediator.Send(new GetUserPictureByIdQuery(pictureId), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else if (result.Status == ResultStatus.NotFound)
        {
            return NotFound(result.Errors);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
