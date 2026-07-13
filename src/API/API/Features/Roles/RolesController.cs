namespace API.Features.Roles;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RequireAdminRole")]
public class RolesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets all available roles.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of roles.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRolesQuery(), cancellationToken);

        if (!result.IsSuccess)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <param name="role">The role details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created role.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create(RoleRequestDTO role, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateRoleCommand(role.Name), cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        else
        {
            // @todo: return createdAtAction with route to get the created role
            return Ok(result.Value);
        }
    }

    /// <summary>
    /// Deletes a role by ID.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success status.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRole(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id), cancellationToken);

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
