using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MessagesController(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;

        }
        // GET: api/Messages/inbox/{username}
        [HttpGet("inbox/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetChat(string username)
        {
            var issuerId = User.GetId();
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            return Ok(await unitOfWork.MessageRepository.GetMessagesDTOThreadAsync(issuerId, user.Id));
        }
        // GET: api/Messages/
        public async Task<ActionResult<IEnumerable<MessageDTO>>> ReceiveMessages(string mode, [FromQuery]PaginationParams paginationParams)
        {
            var issuerId = User.GetId();
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(issuerId);
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
            var result = await unitOfWork.MessageRepository.GetAllPagedMessagesDTOForUserAsync(issuerId, option, paginationParams);
            var paginationHeader = new PaginationHeader(result.CurrentPage, result.ItemsPerPage, result.TotalCount, result.TotalPages);
            Response.AddPaginationHeader(paginationHeader);
            return Ok(result);
        }
        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> PostMessage(NewMessageDTO message)
        {
            var sender = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());
            var recipient = await unitOfWork.UserRepository.GetUserByUsernameAsync(message.RecipientUsername);
            if(recipient == null || sender == null)
            {
                return NotFound();
            }
            if(sender.Id == recipient.Id)
            {
                return BadRequest("You can't send messages to yourself");
            }
            if (!await unitOfWork.LikesRepository.IsMacth(sender.Id, recipient.Id))
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
            unitOfWork.MessageRepository.AddMessage(createdMessage);
            var msgDTO = mapper.Map<MessageDTO>(createdMessage);
            var profilePhoto = await unitOfWork.UserRepository.GetProfilePhotoAsync(sender.Id);
            if(profilePhoto != null)
            {
                msgDTO.SenderPhotoUrl = profilePhoto.Url;
            }
            if (await unitOfWork.Complete())
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
            var message = await unitOfWork.MessageRepository.GetMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            var issuerId = User.GetId();
            unitOfWork.MessageRepository.DeleteMessage(message, issuerId);

            if (await unitOfWork.Complete()) { 
                return NoContent();
            }
            return BadRequest("Failed to delete the message");
        }
    }
}
