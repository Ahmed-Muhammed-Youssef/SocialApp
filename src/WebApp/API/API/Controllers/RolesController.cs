using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.User;
using Shared.Extensions;
using Infrastructure.Identity;
using Application.DTOs.Pagination;
using Application.DTOs.Role;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController(RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager, IdentityDatabaseContext _identityDatabase) : ControllerBase
    {

        // GET: roles/all
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            var result = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Ok(result);
        }

        // GET: roles/users-roles/all
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
                   Email = g.User.Email,
                   Roles = g.Roles
               });

            int totalNumber = await groupedUsers.CountAsync();

            List<UserWithRolesDTO> paginatedResult = await groupedUsers
                .OrderBy(i => i.Email)
                .Skip(pagination.SkipValue)
                .Take(pagination.ItemsPerPage)
                .ToListAsync();
            
            return Ok(new PagedList<UserWithRolesDTO>(paginatedResult, totalNumber, pagination.PageNumber, pagination.ItemsPerPage));
        }

        // GET: roles/user/{email}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("user/{email}")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
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
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleRequestDTO role)
        {
            var newRole = new IdentityRole() { Name = role.Name };
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
                return BadRequest("Failed To create a new role");
            }
            return Ok();
        }

        // DELETE: roles/delete/{role}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(RoleRequestDTO role)
        {
            var appRole = await _roleManager.FindByNameAsync(role.Name);
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
                return BadRequest("Failed To create a new role");
            }
            return Ok();
        }
    }
}
