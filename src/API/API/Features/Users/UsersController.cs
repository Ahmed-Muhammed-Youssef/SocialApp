namespace API.Features.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(LogUserActivity))]
public class UsersController(JsonSerializerOptions jsonSerializerOptions, IMediator mediator) : ControllerBase
{
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] GetUsersRequest request, IValidator<GetUsersRequest> validator, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);

        if(!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

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
    public async Task<ActionResult<UserDTO>> GetUser(int id, CancellationToken cancellationToken)
    {
        Result<UserDTO> result = await mediator.Send(new GetUserQuery(id), cancellationToken);

        if(result.IsSuccess)
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
}
