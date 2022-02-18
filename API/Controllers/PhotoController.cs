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
    public class PhotoController : ControllerBase
    {
        private readonly DataContext _context;

        public PhotoController(DataContext context)
        {
            _context = context;
        }

        /*// GET: api/Photo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhoto()
        {
            return await _context.Photo.ToListAsync();
        }*/

        // GET: api/Photo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhoto(int id)
        {
            var query = _context.Photo.Where(p => p.AppUserId == id);
            var photos = await query.ToListAsync();
            if (photos == null)
            {
                return NotFound();
            }

            return photos;
        }

        /*// PUT: api/Photo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoto(int id, Photo photo)
        {
            if (id != photo.Id)
            {
                return BadRequest();
            }

            _context.Entry(photo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Photo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("upload")]
        [HttpPost]
        public async Task<ActionResult<Photo>> PostPhoto(PhotoReceivedDTO photoDTO)
        {
            if (!UserExists(photoDTO.AppUserId))
            {
                return BadRequest();
            }
            var photo = PhotoReceivedDTOToPhoto(photoDTO);
            // check the order of the new photo
            var query = _context.Photo.Where(p => p.AppUserId == photoDTO.AppUserId);
            var photos = await query.ToListAsync();
            photo.Order = photos.Count();




            _context.Photo.Add(photo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhoto", new { id = photo.Id }, photoDTO);
        }

        // DELETE: api/Photo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            _context.Photo.Remove(photo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.Id == id);
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        private Photo PhotoReceivedDTOToPhoto(PhotoReceivedDTO photoReceivedDTO) => new Photo()
        {
            AppUserId = photoReceivedDTO.AppUserId,
            Url = photoReceivedDTO.Url,
            PublicId = photoReceivedDTO.Url // placeholder
        };
    }
}
