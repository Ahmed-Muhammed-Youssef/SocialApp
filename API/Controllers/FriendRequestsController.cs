﻿using API.DTOs;
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
        private readonly IUnitOfWork unitOfWork;
        public FriendRequestsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // @ToDo: add pagination for scaling
        // GET: api/friendrequests/sent
        [HttpGet("sent")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFriendRequestedUsers()
        {
            var sender = await unitOfWork.UsersRepository.GetUserByIdAsync(User.GetId());
            if(sender == null)
            {
                return BadRequest();
            }
            var friendRequestedUsers = await unitOfWork.FriendRequestsRepository.GetFriendRequestedUsersDTOAsync(sender.Id);
            return Ok(friendRequestedUsers);
        }

        // GET: api/friendrequests/isSent/{username}
        [HttpGet("issent/{username}")]
        public async Task<ActionResult<bool>> IsFriendRequested (string username)
        {
            var sender = await unitOfWork.UsersRepository.GetUserByIdAsync(User.GetId());
            var target = await unitOfWork.UsersRepository.GetUserByUsernameAsync(username);
            if (sender == null || target == null)
            {
                return NotFound();
            }
            if (sender.Id == target.Id)
            {
                return BadRequest("you can't check liking yourself.");
            }
            if (await unitOfWork.FriendRequestsRepository.GetFriendRequestAsync(sender.Id, target.Id) != null)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        // POST: api/friendrequests/send/{username}
        [HttpPost("send/{username}")]
        public async Task<ActionResult> SendFriendRequest(string username)
        {
            // @ToDo: check if the user is already a friend
            if(username == null)
            {
                return BadRequest();
            }
            var sender = await unitOfWork.UsersRepository.GetUserByIdAsync(User.GetId());
            var target = await unitOfWork.UsersRepository.GetUserByUsernameAsync(username);
            if(sender == null || target == null)
            {
                return NotFound();
            }
            if(sender.Id == target.Id)
            {
                return BadRequest("You can't send friend requests to yourself.");
            }
            if(await unitOfWork.FriendRequestsRepository.GetFriendRequestAsync(sender.Id, target.Id) != null)
            {
                return BadRequest("You already sent a frient request to this user.");
            }
            bool isFriend = await unitOfWork.FriendRequestsRepository.SendFriendRequest(sender.Id, target.Id);
            if( await unitOfWork.Complete())
            {
                return Ok(isFriend);
            }
            return BadRequest();
        }
    }
}