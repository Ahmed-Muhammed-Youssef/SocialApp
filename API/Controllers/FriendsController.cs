using API.Application.DTOs;
using API.Application.Interfaces;
using API.Extensions;
using API.Filters;
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
        private readonly IUnitOfWork _unitOfWork;

        public FriendsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/friends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            if (user == null)
            {
                return Unauthorized();
            }
            var friends = await _unitOfWork.FriendRequestRepository.GetFriendsAsync(user.Id, paginationParams);
            var newPaginationHeader = new PaginationHeader(friends.CurrentPage, friends.ItemsPerPage, friends.TotalCount, friends.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader);
            return Ok(friends);
        }

        // GET: api/friends/isfriend/{id}
        [HttpGet("isFriend/{id}")]
        public async Task<ActionResult<bool>> IsFriend(int id)
        {
            return Ok(await _unitOfWork.FriendRequestRepository.IsFriend(User.GetId(), id));
        }
    }
}
