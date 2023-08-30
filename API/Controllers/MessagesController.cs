using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessagesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // DELETE: api/Messages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.MessagesRepository.GetMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            var issuerId = User.GetId();

            // Checks if the message is related to the issuer
            if (message.SenderId != issuerId && message.RecipientId != issuerId)
            {
                return BadRequest("Failed to delete the message");
            }
            _unitOfWork.MessagesRepository.DeleteMessage(message, issuerId);

            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }
            return BadRequest("Failed to delete the message");
        }
    }
}
