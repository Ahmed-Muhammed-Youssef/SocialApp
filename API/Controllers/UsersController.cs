using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using API.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.ConvertAll(AppUsertoDTO);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
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
            if (AppUserExists(id))
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
                if (!AppUserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostAppUser(UserDTO user)
        {
            var userToPost = DTOtoAppUser(user);
            _context.Users.Add(userToPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostAppUser", new { id = userToPost.Id }, user);
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

        private bool AppUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
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
