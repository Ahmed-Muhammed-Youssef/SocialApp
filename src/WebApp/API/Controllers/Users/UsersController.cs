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
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] GetUsersRequest request, IValidator<GetUsersRequest> validator)
    {
        var validationResult = validator.Validate(request);

        if(!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await mediator.Send(new GetUsersQuery(request.UserParams));

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
    public async Task<ActionResult<UserDTO>> GetUser(int id)
    {
        Result<UserDTO> result = await mediator.Send(new GetUserQuery(id));

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
    public async Task<ActionResult<UserDTO>> Update(UpdateUserRequest userDTO)
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
        });

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
