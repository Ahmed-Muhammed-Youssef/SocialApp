namespace API.Controllers.Roles;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController(RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager, IdentityDatabaseContext _identityDatabase, IMediator mediator) : ControllerBase
{

    // GET: api/roles/all
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles()
    {
        var result = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        return Ok(result);
    }

    // GET: api/roles/users-roles/all
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-roles/all")]
    public async Task<ActionResult<PagedList<UserWithRolesDTO>>> GetUsersRoles([FromQuery] PaginationParams pagination)
    {
        var groupedUsers = _identityDatabase.Users
           .GroupJoin(
               _identityDatabase.UserRoles,
               user => user.Id,
               userRole => userRole.UserId,
               (user, userRoles) => new
               {
                   User = user,
                   Roles = userRoles
                       .Join(_identityDatabase.Roles,
                             userRole => userRole.RoleId,
                             role => role.Id,
                             (userRole, role) => role.Name)
                       .ToList()
               })
           .Select(g => new UserWithRolesDTO
           {
               Email = g.User!.Email!,
               Roles = g.Roles!
           });

        int totalNumber = await groupedUsers.CountAsync();

        List<UserWithRolesDTO> paginatedResult = await groupedUsers
            .OrderBy(i => i.Email)
            .Skip(pagination.SkipValue())
            .Take(pagination.ItemsPerPage)
            .ToListAsync();
        
        return Ok(new PagedList<UserWithRolesDTO>(paginatedResult, totalNumber, pagination.PageNumber, pagination.ItemsPerPage));
    }

    // GET: api/roles/user
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("user")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles([FromBody]string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var result = await _userManager.GetRolesAsync(user);

        return Ok(result);
    }

    // GET: api/roles
    [HttpGet]
    public ActionResult<IEnumerable<string>> GetMyRoles()
    {
        var result = User.GetRoles();

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

    // POST: roles/add
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("add")]
    public async Task<IActionResult> AddRoleToUser(RoleUserDTO roleUser)
    {
        var user = await _userManager.FindByEmailAsync(roleUser.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var Role = await _roleManager.FindByNameAsync(roleUser.Role);
        if (Role == null)
        {
            return NotFound("Role not found");
        }
        var result = await _userManager.AddToRoleAsync(user, roleUser.Role);
        if (!result.Succeeded)
        {
            return BadRequest(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
        }
        return Ok();
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

    // DELETE: roles/removefrom
    [Authorize(Policy = "RequireAdminRole")]
    [HttpDelete("removefrom")]
    public async Task<IActionResult> RemoveRoleFromUser(RoleUserDTO roleUser)
    {
        var user = await _userManager.FindByEmailAsync(roleUser.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var Role = await _roleManager.FindByNameAsync(roleUser.Role);
        if (Role == null)
        {
            return NotFound("Role not found");
        }
        var result = await _userManager.RemoveFromRoleAsync(user, roleUser.Role);
        if (!result.Succeeded)
        {
            return BadRequest(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
        }
        return Ok();
    }
}
