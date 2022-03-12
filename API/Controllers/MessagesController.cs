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

        public MessagesController(IMessageRepository messageRepository, IUserRepository userRepository,
             IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        // GET: api/Messages
        [HttpGet("inbox/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetChat(string username)
        {
            var issuerId = User.GetId();
            var user = await userRepository.GetUserByUsernameAsync(username);
            return Ok(await messageRepository.GetMessagesDTOThreadAsync(issuerId, user.Id));
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
            msgDTO.SenderPhotoUrl = profilePhoto.Url;
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
