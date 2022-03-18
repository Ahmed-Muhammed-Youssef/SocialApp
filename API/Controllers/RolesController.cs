using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [Authorize(Policy="RequireAdminRole")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            var result = await roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Ok(result);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-roles/all")]
        public async Task<ActionResult> GetUsersRoles()
        {
            var result = await userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => 
                new {
                u.Id,
                Username = u.UserName,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();
            return Ok(result);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await userManager.GetRolesAsync(user);
           
            return Ok(result);
        }
        [HttpGet("user")]
        public ActionResult<IEnumerable<string>> GetMyRoles()
        {
            var result = User.GetRoles();

            return Ok(result);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create/{role}")]
        public async Task<IActionResult>  CreateRole(string role)
        {
            var newRole = new AppRole() { Name = role};
            var result = await roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
            {
                return BadRequest("Failed To create a new role");
            }
            return Ok(role);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("add")]
        public async Task<IActionResult> AddRoleToUser([Required, FromQuery] string username, [Required, FromQuery] string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if(user == null)
            {
                return NotFound("User not found");
            }
            var Role = await roleManager.FindByNameAsync(role);
            if(Role == null)
            {
                return NotFound("Role not found");
            }
            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return BadRequest("Failed To create a new role");
            }
            return Ok(role);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete/{role}")]
        public async Task<IActionResult> DeleteRole(string role)
        {
            var appRole = await roleManager.FindByNameAsync(role);
            if (appRole == null)
            {
                return NotFound("Role not found");
            }
            var result = await roleManager.DeleteAsync(appRole);
            if (!result.Succeeded)
            {
                return BadRequest("Failed To create a new role");
            }
            return NoContent();
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("removefrom")]
        public async Task<IActionResult> RemoveRoleFromUser([Required, FromQuery] string username, [Required, FromQuery] string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var Role = await roleManager.FindByNameAsync(role);
            if (Role == null)
            {
                return NotFound("Role not found");
            }
            var result = await userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return BadRequest("Failed To create a new role");
            }
            return Ok(role);
        }
    }
}
