namespace API.Controllers.Roles;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController(RoleManager<IdentityRole> _roleManager, IMediator mediator) : ControllerBase
{

    // GET: api/roles/all
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles()
    {
        var result = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        return Ok(result);
    }

    // POST: api/api/roles
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost]
    public async Task<IActionResult> Create(RoleRequestDTO role)
    {
        var result = await mediator.Send(new CreateRoleCommand(role.Name));
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
    [Authorize(Policy = "RequireAdminRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id));

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
