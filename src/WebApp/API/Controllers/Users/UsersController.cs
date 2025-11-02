using API.Common.Extensions;
using API.Common.Filters;
using FluentValidation;

namespace API.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(LogUserActivity))]
public class UsersController(JsonSerializerOptions jsonSerializerOptions, IMediator mediator) : ControllerBase
{

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] GetUsersRequest request, CancellationToken cancellationToken, IValidator<GetUsersRequest> validator)
    {
        var validationResult = validator.Validate(request);

        if(!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await mediator.Send(new GetUsersQuery(request.UserParams), cancellationToken);

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
}
