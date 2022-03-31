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
        private readonly IUnitOfWork unitOfWork;

        public LikeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet("liked")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetLikedUsers()
        {
            var liker = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            if(liker == null)
            {
                return BadRequest();
            }
            var likedUsers = await unitOfWork.LikesRepository.GetLikedUsersDTOAsync(liker.Id);
            return Ok(likedUsers);
        }
        [HttpGet("isLiked/{username}")]
        public async Task<ActionResult<bool>> IsLiked (string username)
        {
            var liker = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            var liked = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (liker == null || liked == null)
            {
                return NotFound();
            }
            if (liker.Id == liked.Id)
            {
                return BadRequest("you can't check liking yourself.");
            }
            if (await unitOfWork.LikesRepository.GetLikeAsync(liker.Id, liked.Id) != null)
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
            var liker = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            var liked = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if(liker == null || liked == null)
            {
                return NotFound();
            }
            if(liker.Id == liked.Id)
            {
                return BadRequest("You can't like yourself.");
            }
            if(await unitOfWork.LikesRepository.GetLikeAsync(liker.Id, liked.Id) != null)
            {
                return BadRequest("You already liked this user.");
            }
            bool isMatch = await unitOfWork.LikesRepository.LikeAsync(liker.Id, liked.Id);
            if( await unitOfWork.Complete())
            {
                return Ok(isMatch);
            }
            return BadRequest();
        }
    }
}
