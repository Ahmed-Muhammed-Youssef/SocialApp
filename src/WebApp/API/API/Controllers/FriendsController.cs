using Application.DTOs.Pagination;
using Application.DTOs.User;
using Application.Interfaces;
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

    public class FriendsController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        // GET: api/friends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetId());
            if (user == null)
            {
                return Unauthorized();
            }
            var friends = await _unitOfWork.FriendRequestRepository.GetFriendsAsync(user.Id, paginationParams);
            var newPaginationHeader = new PaginationHeader(friends.CurrentPage, friends.ItemsPerPage, friends.Count, friends.TotalPages);
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
