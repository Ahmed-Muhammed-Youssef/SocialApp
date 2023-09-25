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
    public class FriendRequestsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public FriendRequestsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFriendRequests()
        {
            var sender = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            if (sender == null)
            {
                return BadRequest();
            }
            var friendRequests = await _unitOfWork.FriendRequestRepository.GetRecievedFriendRequestsAsync(sender.Id);
            return Ok(friendRequests);
        }

        [HttpPost("cancel/{username}")]
        public async Task<IActionResult> CancelFriendRequests(string username)
        {
            if (username is null)
            {
                return BadRequest("invalid username");
            }
            var sender = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            var target = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (sender == null || target == null)
            {
                return NotFound();
            }
            if (sender.Id == target.Id)
            {
                return BadRequest("You can't send friend requests to yourself.");
            }
            if (await _unitOfWork.FriendRequestRepository.IsFriend(sender.Id, target.Id) == true)
            {
                return BadRequest("You already are friends.");
            }
            // check if the api issuer is the freind request sender first.
            var fr = await _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(sender.Id, target.Id);
            if (fr is null)
            {
                // check if the api issuer is the freind requested user
                fr = await _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(target.Id, sender.Id);
            }
            if (fr is null)
            {
                return BadRequest("You didn't sent a friend request.");
            }
            _unitOfWork.FriendRequestRepository.DeleteFriendRequest(fr);
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Failed to cancel friend request.");
        }
        // @ToDo: add pagination for scaling
        // GET: api/friendrequests/sent
        [HttpGet("sent")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFriendRequestedUsers()
        {
            var sender = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            if (sender == null)
            {
                return BadRequest();
            }
            var friendRequestedUsers = await _unitOfWork.FriendRequestRepository.GetFriendRequestedUsersDTOAsync(sender.Id);
            return Ok(friendRequestedUsers);
        }

        // GET: api/friendrequests/isSent/{username}
        [HttpGet("issent/{username}")]
        public async Task<ActionResult<bool>> IsFriendRequested(string username)
        {
            var senderId = User.GetId();
            var target = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (target == null)
            {
                return NotFound();
            }
            if (senderId == target.Id)
            {
                return BadRequest("you can't check liking yourself.");
            }
            if (await _unitOfWork.FriendRequestRepository.IsFriendRequestedAsync(senderId, target.Id))
            {
                return Ok(true);
            }

            return Ok(false);
        }

        // POST: api/friendrequests/send/{username}
        [HttpPost("send/{username}")]
        public async Task<ActionResult> SendFriendRequest(string username)
        {
            // retuns true if the user has become a frined
            if (username == null)
            {
                return BadRequest();
            }
            var sender = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            var target = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (sender == null || target == null)
            {
                return NotFound();
            }
            if (sender.Id == target.Id)
            {
                return BadRequest("You can't send friend requests to yourself.");
            }
            if (await _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(sender.Id, target.Id) != null)
            {
                return BadRequest("You already sent a frient request to this user.");
            }
            if (await _unitOfWork.FriendRequestRepository.IsFriend(sender.Id, target.Id) == true)
            {
                return BadRequest("You already are friends.");
            }
            bool isFriend = await _unitOfWork.FriendRequestRepository.SendFriendRequest(sender.Id, target.Id);
            if (await _unitOfWork.Complete())
            {
                return Ok(isFriend);
            }
            return BadRequest();
        }
    }
}
