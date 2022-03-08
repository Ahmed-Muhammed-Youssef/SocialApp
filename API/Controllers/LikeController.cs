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
        [HttpGet("liked")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetLikedUsers()
        {
            var liker = await userRepository.GetUserByIdAsync(User.GetId());
            if(liker == null)
            {
                return BadRequest();
            }
            var likedUsers = await likesRepository.GetLikedUsersDTOAsync(liker.Id);
            return Ok(likedUsers);
        }
        [HttpGet("isLiked/{username}")]
        public async Task<ActionResult<bool>> IsLiked (string username)
        {
            var liker = await userRepository.GetUserByIdAsync(User.GetId());
            var liked = await userRepository.GetUserByUsernameAsync(username);
            if (liker == null || liked == null)
            {
                return NotFound();
            }
            if (liker.Id == liked.Id)
            {
                return BadRequest("you can't check liking yourself.");
            }
            if (await likesRepository.GetLikeAsync(liker.Id, liked.Id) != null)
            {
                return Ok(true);
            }
            return Ok(false);
        }
        [HttpPost("{username}")]
        public async Task<ActionResult> PostLike(string username)
        {
            if(username == null)
            {
                return BadRequest();
            }
            var liker = await userRepository.GetUserByIdAsync(User.GetId());
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
