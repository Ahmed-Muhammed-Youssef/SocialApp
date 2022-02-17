using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet("all")]
        [AllowAnonymous] // for testing
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.ConvertAll(AppUsertoDTO);
        }

        // GET: api/Users/5
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var appUser = await _context.Users.FindAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return AppUsertoDTO(appUser);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppUser(int id, UserDTO user)
        {
            if (await AppUserExists(id))
            {
                return BadRequest();
            }

            _context.Entry(DTOtoAppUser(user)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AppUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppUser(int id)
        {
            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(appUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> AppUserExists(int id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }
        private AppUser DTOtoAppUser(UserDTO user) =>
            new AppUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Sex = user.Sex,
                Interest = user.Interest
            };
        private UserDTO AppUsertoDTO(AppUser user) =>
            new UserDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Sex = user.Sex,
                Interest = user.Interest
            };
    }
}
