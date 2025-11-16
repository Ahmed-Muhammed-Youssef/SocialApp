namespace API.Features.Users;

[ApiController]
[Authorize(Policy = "RequireAdminRole")]
[Route("api/users/{userId}/roles")]
public class UserRolesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetUserRoles(int userId)
    {
        Result<List<string>> result = await mediator.Send(new GetUserRolesQuery(userId));

        if(result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound(result.Errors);
        }
    }

    [HttpPost("{roleId}")]
    public async Task<ActionResult> AssignRoleToUser(int userId, string roleId)
    {
        Result<object?> result = await mediator.Send(new AssignRoleToUserCommand(userId, roleId));

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

    [HttpDelete("{roleId}")]
    public async Task<ActionResult> RemoveRoleFromUser(int userId, string roleId)
    {
        Result<object?> result = await mediator.Send(new RemoveRoleFromUserCommand(userId, roleId));
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
