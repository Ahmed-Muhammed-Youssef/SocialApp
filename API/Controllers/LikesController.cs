using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly DataContext _context;

        public LikesController(DataContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Like>>> PostLike(IdPairs like)
        {
            await _context.Likes.AddAsync(DTOtoLike(like));
            // check if the liked id likes him or not if it does, it's a match
            bool isMatch = await _context.Likes.AnyAsync(l => l.LikerId == like.SecondId 
            && l.LikedId == like.FirstId);
            if (isMatch)
            {
                await _context.Matches.AddAsync(new Match { UserId = like.FirstId, MatchedId = like.SecondId });
                await _context.Matches.AddAsync(new Match { UserId = like.SecondId, MatchedId = like.FirstId });
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("PostLike", new { Likedid = like.SecondId, isMatch }, like);
        }
        private Like DTOtoLike(IdPairs like) => new Like()
        {
            LikerId = like.FirstId,
            LikedId = like.SecondId
        };
    }
}
