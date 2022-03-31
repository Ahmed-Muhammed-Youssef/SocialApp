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
        private readonly IUnitOfWork unitOfWork;

        public MatchesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllMatches([FromQuery]PaginationParams paginationParams)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user == null)
            {
                return Unauthorized();
            }
            var matches = await unitOfWork.LikesRepository.GetMatchesAsync(user.Id, paginationParams);
            var newPaginationHeader = new PaginationHeader(matches.CurrentPage, matches.ItemsPerPage, matches.TotalCount, matches.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader);
            return Ok(matches);
        }
        [HttpGet("isMatch/{username}")]
        public async Task<ActionResult<bool>> IsMatch(string username)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var otherUser = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if(otherUser == null)
            {
                return NotFound();
            }
            return Ok(await unitOfWork.LikesRepository.IsMacth(user.Id, otherUser.Id));

        }
    }
}
