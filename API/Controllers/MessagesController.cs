using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using API.Interfaces;
using API.Extensions;
using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using API.Helpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository messageRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILikesRepository likesRepository;

        public MessagesController(IMessageRepository messageRepository, IUserRepository userRepository,
             IMapper mapper, ILikesRepository likesRepository)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.likesRepository = likesRepository;
        }
        // GET: api/Messages/inbox/{username}
        [HttpGet("inbox/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetChat(string username)
        {
            var issuerId = User.GetId();
            var user = await userRepository.GetUserByUsernameAsync(username);
            return Ok(await messageRepository.GetMessagesDTOThreadAsync(issuerId, user.Id));
        }
        // GET: api/Messages/
        public async Task<ActionResult<IEnumerable<MessageDTO>>> ReceiveMessages(string mode, [FromQuery]PaginationParams paginationParams)
        {
            var issuerId = User.GetId();
            var user = await userRepository.GetUserByIdAsync(issuerId);
            if(user == null)
            {
                return BadRequest("Invalid Token");
            }

            ReceiveMessagesOptions option = ReceiveMessagesOptions.AllMessages;

            if(mode == "sent")
            {
                option = ReceiveMessagesOptions.SentMessages;
            }
            else if(mode == "received")
            {
                option = ReceiveMessagesOptions.ReceivedMessages;
            }
            else if(mode == "unread")
            {
                option = ReceiveMessagesOptions.UnreadMessages;
            }
            var result = await messageRepository.GetAllPagedMessagesDTOForUserAsync(issuerId, option, paginationParams);
            var paginationHeader = new PaginationHeader(result.CurrentPage, result.ItemsPerPage, result.TotalCount, result.TotalPages);
            Response.AddPaginationHeader(paginationHeader);
            return Ok(result);
        }
        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> PostMessage(NewMessageDTO message)
        {
            var sender = await userRepository.GetUserByIdAsync(User.GetId());
            var recipient = await userRepository.GetUserByUsernameAsync(message.RecipientUsername);
            if(recipient == null || sender == null)
            {
                return NotFound();
            }
            if(sender.Id == recipient.Id)
            {
                return BadRequest("You can't send messages to yourself");
            }
            if (!await likesRepository.IsMacth(sender.Id, recipient.Id))
            {
                return BadRequest("You can't send messages to an unmatch");

            }
            var createdMessage = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Sender = sender,
                Recipient = recipient,
                Content = message.Content,
                SenderDeleted = false,
                RecipientDeleted = false,
                ReadDate = null
            };
            messageRepository.AddMessage(createdMessage);
            var msgDTO = mapper.Map<MessageDTO>(createdMessage);
            var profilePhoto = await userRepository.GetProfilePhotoAsync(sender.Id);
            if(profilePhoto != null)
            {
                msgDTO.SenderPhotoUrl = profilePhoto.Url;
            }
            if (await messageRepository.SaveAsync())
            {
                msgDTO.Id = createdMessage.Id;
                return Ok(msgDTO);
            }
            return BadRequest(); 
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await messageRepository.GetMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            var issuerId = User.GetId();
            messageRepository.DeleteMessage(message, issuerId);

            if (await messageRepository.SaveAsync()) { 
                return NoContent();
            }
            return BadRequest("Failed to delete the message");
        }
    }
}
