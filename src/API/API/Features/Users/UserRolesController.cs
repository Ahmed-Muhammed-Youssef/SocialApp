namespace API.Features.Users;

[ApiController]
[Authorize(Policy = "RequireAdminRole")]
[Route("api/users/{userId}/roles")]
public class UserRolesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets the roles assigned to a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of assigned role names.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<string>>> GetUserRoles(int userId)
    {
        Result<List<string>> result = await mediator.Send(new GetUserRolesQuery(userId));

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound(result.Errors);
        }
    }

    /// <summary>
    /// Assigns a role to a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="roleId">The role name.</param>
    /// <returns>Success status.</returns>
    [HttpPost("{roleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Removes a role from a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="roleId">The role name.</param>
    /// <returns>Success status.</returns>
    [HttpDelete("{roleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
