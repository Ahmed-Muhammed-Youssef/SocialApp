using Application.Features.Roles.List;

namespace API.Controllers.Roles;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RequireAdminRole")]
public class RolesController(IMediator mediator) : ControllerBase
{
    // GET: api/roles
    [HttpGet]
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

    // POST: api/api/roles
    [HttpPost]
    public async Task<IActionResult> Create(RoleRequestDTO role, CancellationToken cancellationToken)
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
    
    // DELETE: api/roles/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id), cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent(); 
        }
        else if(result.Status == ResultStatus.NotFound)
        {
            return NotFound(result.Errors);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
