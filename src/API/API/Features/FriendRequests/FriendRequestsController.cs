namespace API.Features.FriendRequests;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogUserActivity))]
public class FriendRequestsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets pending friend requests for the current user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of pending friend requests.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<UserDTO>>> GetFriendRequests(CancellationToken cancellationToken)
    {
        Result<List<UserDTO>> result = await mediator.Send(new GetFriendRequstsQuery(), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Cancels or declines a friend request.
    /// </summary>
    /// <param name="id">The friend request ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success status.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        Result<object?> result = await mediator.Send(new DeleteFriendRequestCommand(id), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    /// <summary>
    /// Sends a friend request to a user.
    /// </summary>
    /// <param name="userId">The ID of the user to send the request to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the created friend request.</returns>
    [HttpPost("{userId}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Create(int userId, CancellationToken cancellationToken)
    {
        Result<int> result = await mediator.Send(new CreateFriendRequestCommand(userId), cancellationToken);
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
    /// Accepts a friend request.
    /// </summary>
    /// <param name="id">The friend request ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The accepted friend request details.</returns>
    [HttpPut("{id}/accept")]
    [ProducesResponseType(typeof(FriendCreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FriendCreatedResponse>> Accept(int id, CancellationToken cancellationToken)
    {
        Result<FriendCreatedResponse> result = await mediator.Send(new AcceptFriendRequestCommand(id), cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
