using Application.DTOs.Post;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Extensions;

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
            var posts = await _unitOfWork.PostRepository.GetUserPostsAsync(userId, User.GetPublicId().Value);
            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostByIdAsync(ulong postId)
        {
            var posts = await _unitOfWork.PostRepository.GetByIdAsync(postId, User.GetPublicId().Value);
            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult<PostDTO>> AddPost([FromBody]AddPostDTO newPostDTO)
        {
            Post post = _mapper.Map<Post>(newPostDTO);
            post.UserId = User.GetPublicId().Value;

            await _unitOfWork.PostRepository.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();

            PostDTO returnPostDTO = _mapper.Map<PostDTO>(post);
            returnPostDTO.Id = post.Id;

            return Ok(returnPostDTO);
        }
    }
}
