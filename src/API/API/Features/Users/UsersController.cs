namespace API.Features.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(LogUserActivity))]
public class UsersController(JsonSerializerOptions jsonSerializerOptions, IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets a paginated list of users.
    /// </summary>
    /// <param name="request">The pagination and filtering parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of users.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Updates the current user's profile.
    /// </summary>
    /// <param name="userDTO">The updated user details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated user details.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Sets the current user's active profile picture.
    /// </summary>
    /// <param name="pictureId">The picture ID to set as active.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success status.</returns>
    [HttpPost]
    [Route("set-profile-picture/{pictureId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Gets posts created by a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of posts.</returns>
    [HttpGet("{userId}/posts")]
    [ProducesResponseType(typeof(List<PostDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Uploads a new profile picture.
    /// </summary>
    /// <param name="file">The picture file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the uploaded picture.</returns>
    [HttpPost("user-pictures")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateUserPicture(IFormFile file, CancellationToken cancellationToken)
    {
        Result<int> result = await mediator.Send(new CreateUserPictureCommand(file), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
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

    /// <summary>
    /// Deletes a profile picture.
    /// </summary>
    /// <param name="pictureId">The picture ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success status.</returns>
    [HttpDelete("user-pictures/{pictureId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteUserPicture(int pictureId, CancellationToken cancellationToken)
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

    /// <summary>
    /// Gets all profile pictures for the current user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of profile pictures.</returns>
    [HttpGet("user-pictures")]
    [ProducesResponseType(typeof(IEnumerable<PictureDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PictureDTO>>> GetUserPictures(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserPicturesQuery(), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Gets a specific profile picture by ID.
    /// </summary>
    /// <param name="pictureId">The picture ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The profile picture details.</returns>
    [HttpGet("user-pictures/{pictureId}")]
    [ProducesResponseType(typeof(PictureDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
