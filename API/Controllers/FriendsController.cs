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

    public class FriendsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public FriendsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/friends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll([FromQuery]PaginationParams paginationParams)
        {
            var user = await unitOfWork.UsersRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user == null)
            {
                return Unauthorized();
            }
            var friends = await unitOfWork.FriendRequestsRepository.GetFriendsAsync(user.Id, paginationParams);
            var newPaginationHeader = new PaginationHeader(friends.CurrentPage, friends.ItemsPerPage, friends.TotalCount, friends.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader);
            return Ok(friends);
        }

        // GET: api/friends/isfriend/{username}
        [HttpGet("isFriend/{username}")]
        public async Task<ActionResult<bool>> IsFriend(string username)
        {
            var user = await unitOfWork.UsersRepository.GetUserByUsernameAsync(User.GetUsername());
            var target = await unitOfWork.UsersRepository.GetUserByUsernameAsync(username);
            if(target == null)
            {
                return NotFound();
            }
            return Ok(await unitOfWork.FriendRequestsRepository.IsFriend(user.Id, target.Id));

        }
    }
}
