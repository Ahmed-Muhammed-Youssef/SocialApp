using API.Application.Interfaces;
using API.Domain.Entities;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetUserPostsAsync([FromQuery]int userId)
        {
            var posts = await _unitOfWork.PostRepository.GetUserPostsAsync(userId, User.GetId());
            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostByIdAsync(ulong postId)
        {
            var posts = await _unitOfWork.PostRepository.GetPostByIdAsync(postId, User.GetId());
            return Ok(posts);
        }
    }
}
