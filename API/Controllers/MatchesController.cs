using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MatchesController : ControllerBase
    {
        private readonly ILikesRepository likesRepository;
        private readonly IUserRepository userRepository;

        public MatchesController(ILikesRepository likesRepository, IUserRepository userRepository)
        {
            this.likesRepository = likesRepository;
            this.userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllMatches()
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user == null)
            {
                return Unauthorized();
            }
            var matches = await likesRepository.GetMatchesAsync(user.Id);
            return Ok(matches);
        }
    }
}
