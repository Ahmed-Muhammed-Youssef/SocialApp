using Application.DTOs.Post;
using Application.Interfaces;
using Domain.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController(IUnitOfWork _unitOfWork, IMapper _mapper) : ControllerBase
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

        [HttpPost]
        public async Task<ActionResult<PostDTO>> AddPost([FromBody]AddPostDTO newPostDTO)
        {
            Post post = _mapper.Map<Post>(newPostDTO);
            post.UserId = User.GetId();

            await _unitOfWork.PostRepository.AddPostAsync(post);
            await _unitOfWork.Complete();

            PostDTO returnPostDTO = _mapper.Map<PostDTO>(post);
            returnPostDTO.Id = post.Id;

            return Ok(returnPostDTO);
        }
    }
}
