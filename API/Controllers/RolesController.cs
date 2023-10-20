using API.Domain.Entities;
using API.DTOs;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: roles/all
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            var result = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Ok(result);
        }

        // @ToDo: add pagination this endpoint takes alot of time
        // GET: roles/users-roles/all
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-roles/all")]
        public async Task<ActionResult> GetUsersRoles()
        {
            var result = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u =>
                new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();
            return Ok(result);
        }

        // GET: roles/user/{username}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await _userManager.GetRolesAsync(user);

            return Ok(result);
        }

        // GET: roles/user
        [HttpGet("user")]
        public ActionResult<IEnumerable<string>> GetMyRoles()
        {
            var result = User.GetRoles();

            return Ok(result);
        }

        // POST: roles/create/{role}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create/{role}")]
        public async Task<IActionResult> CreateRole(string role)
        {
            var newRole = new AppRole() { Name = role };
            var result = await _roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
            {
                return BadRequest("Failed To create a new role");
            }
            return Ok(role);
        }

        // POST: roles/add
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("add")]
        public async Task<IActionResult> AddRoleToUser(RoleUserDTO roleUser)
        {
            var user = await _userManager.FindByNameAsync(roleUser.Username);
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
                return BadRequest("Failed To create a new role");
            }
            return Ok();
        }

        // DELETE: roles/delete/{role}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete/{role}")]
        public async Task<IActionResult> DeleteRole(string role)
        {
            var appRole = await _roleManager.FindByNameAsync(role);
            if (appRole == null)
            {
                return NotFound("Role not found");
            }
            var result = await _roleManager.DeleteAsync(appRole);
            if (!result.Succeeded)
            {
                return BadRequest("Failed To create a new role");
            }
            return NoContent();
        }

        // DELETE: roles/removefrom
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("removefrom")]
        public async Task<IActionResult> RemoveRoleFromUser(RoleUserDTO roleUser)
        {
            var user = await _userManager.FindByNameAsync(roleUser.Username);
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
                return BadRequest("Failed To create a new role");
            }
            return Ok();
        }
    }
}
