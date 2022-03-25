using API.DTOs;
using API.Extensions;
using API.Helpers;
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
    [ServiceFilter(typeof(LogUserActivity))]

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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllMatches([FromQuery]PaginationParams paginationParams)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user == null)
            {
                return Unauthorized();
            }
            var matches = await likesRepository.GetMatchesAsync(user.Id, paginationParams);
            var newPaginationHeader = new PaginationHeader(matches.CurrentPage, matches.ItemsPerPage, matches.TotalCount, matches.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader);
            return Ok(matches);
        }
        [HttpGet("isMatch/{username}")]
        public async Task<ActionResult<bool>> IsMatch(string username)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            var otherUser = await userRepository.GetUserByUsernameAsync(username);
            if(otherUser == null)
            {
                return NotFound();
            }
            return Ok(await likesRepository.IsMacth(user.Id, otherUser.Id));

        }
    }
}
