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
