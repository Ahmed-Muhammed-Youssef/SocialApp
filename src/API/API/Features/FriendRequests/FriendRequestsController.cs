namespace API.Features.FriendRequests;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogUserActivity))]
public class FriendRequestsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
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

    // DELETE: api/friendrequests/{id}
    [HttpDelete("{id}")]
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

    // POST: api/friendrequests/{id}
    [HttpPost("{userId}")]
    public async Task<ActionResult> Create(int userId, CancellationToken cancellationToken)
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

    // PUT: api/friendrequests/{id}/accept
    [HttpPut("{id}/accept")]
    public async Task<ActionResult> Accept(int id, CancellationToken cancellationToken)
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
