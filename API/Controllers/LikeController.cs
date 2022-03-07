using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    public class LikeController : ControllerBase
    {
        private readonly ILikesRepository likesRepository;
        private readonly IUserRepository userRepository;

        public LikeController(ILikesRepository likesRepository, IUserRepository userRepository)
        {
            this.likesRepository = likesRepository;
            this.userRepository = userRepository;
        }
        [HttpPost("{username}")]
        public async Task<ActionResult> PostLike(string username)
        {
            if(username == null)
            {
                return BadRequest();
            }
            var liker = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            var liked = await userRepository.GetUserByUsernameAsync(username);
            if(liker == null || liked == null)
            {
                return NotFound();
            }
            if(liker.Id == liked.Id)
            {
                return BadRequest("You can't like yourself.");
            }
            if(await likesRepository.GetLikeAsync(liker.Id, liked.Id) != null)
            {
                return BadRequest("You already liked this user.");
            }
            bool isMatch = await likesRepository.LikeAsync(liker.Id, liked.Id);
            if( await likesRepository.SaveAllAsync())
            {
                return Ok(isMatch);
            }
            return BadRequest();
        }
    }
}
