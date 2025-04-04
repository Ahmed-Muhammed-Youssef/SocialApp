﻿namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    public class FriendRequestsController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFriendRequests()
        {
            var sender = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId().Value);
            if (sender == null)
            {
                return BadRequest();
            }
            var friendRequests = await _unitOfWork.FriendRequestRepository.GetRecievedFriendRequestsAsync(sender.Id);
            return Ok(friendRequests);
        }
        // DELETE: api/friendrequests/cancel/{id}
        [HttpDelete("cancel/{id}")]
        public async Task<IActionResult> CancelFriendRequests(int id)
        {
            var sender = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId().Value);
            var target = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id);
            if (sender == null || target == null)
            {
                return NotFound();
            }
            if (sender.Id == target.Id)
            {
                return BadRequest();
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

            _unitOfWork.FriendRequestRepository.Delete(fr);
            
            await _unitOfWork.SaveChangesAsync();
            
            return Ok();
        }
        // @ToDo: add pagination for scaling
        // GET: api/friendrequests/sent
        [HttpGet("sent")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFriendRequestedUsers()
        {
            var sender = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId().Value);
            if (sender == null)
            {
                return BadRequest();
            }
            var friendRequestedUsers = await _unitOfWork.FriendRequestRepository.GetFriendRequestedUsersDTOAsync(sender.Id);
            return Ok(friendRequestedUsers);
        }

        // GET: api/friendrequests/isSent/{id}
        [HttpGet("issent/{id}")]
        public async Task<ActionResult<bool>> IsFriendRequested(int id)
        {
            var senderId = User.GetPublicId().Value;
            var target = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id);
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

        // POST: api/friendrequests/send/{id}
        [HttpPost("send/{id}")]
        public async Task<ActionResult> SendFriendRequest(int id)
        {
            // retuns true if the user has become a frined
            var sender = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId().Value);
            var target = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id);
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

            await _unitOfWork.SaveChangesAsync();

            return Ok(isFriend);
        }
    }
}
