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
