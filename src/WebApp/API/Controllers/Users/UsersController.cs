using API.Controllers.Users.Requests;
using Application.Features.Users;
using Application.Features.Users.GetById;
using Application.Features.Users.Update;
using Mediator;
using Shared.Results;

namespace API.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(LogUserActivity))]
public class UsersController(IUnitOfWork _unitOfWork, JsonSerializerOptions jsonSerializerOptions, IMediator mediator) : ControllerBase
{

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] UserParams userParams)
    {
        var users = await _unitOfWork.ApplicationUserRepository.GetUsersDTOAsync(User.GetPublicId(), userParams);
        var newPaginationHeader = new PaginationHeader(users.CurrentPage, users.ItemsPerPage, users.Count, users.TotalPages);
        Response.AddPaginationHeader(newPaginationHeader, jsonSerializerOptions);
        return Ok(users.Items);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(int id)
    {
        Result<UserDTO> result = await mediator.Send(new GetUserByIdQuery(id));

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
