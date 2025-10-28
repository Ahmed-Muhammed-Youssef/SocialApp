namespace API.Controllers.FriendRequests;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogUserActivity))]
public class FriendRequestsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetFriendRequests()
    {
        Result<List<UserDTO>> result = await mediator.Send(new GetFriendRequstsQuery());

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else 
        {
            return BadRequest();
        }
    }

    // DELETE: api/friendrequests/{userId}
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(int userId)
    {
        Result<object?> result = await mediator.Send(new DeleteFriendRequestCommand(userId));

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
    public async Task<ActionResult> Create(int userId)
    {
        Result<int> result = await mediator.Send(new CreateFriendRequestCommand(userId));
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
