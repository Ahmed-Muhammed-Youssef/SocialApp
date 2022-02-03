using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly DataContext context;

        public MatchesController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetAllMatches()
        {
            var matches = await context.Matches.ToListAsync();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches(int id)
        {
            var matches = await context.Matches.AsQueryable()
                .Where(m => m.UserId == id)
                .Select(m => m)
                .ToListAsync();
            return Ok(matches.ConvertAll(MatchtoDTO));
        }
        private Match DTOtoMatch(IdPairs match) => new Match()
        {
            UserId = match.FirstId,
            MatchedId = match.SecondId
        };
        private IdPairs MatchtoDTO(Match match) => new IdPairs()
        {
            FirstId = match.UserId,
            SecondId = match.MatchedId
        };
    }
}
