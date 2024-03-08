using API.Application.Interfaces;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        // DELETE: api/Messages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetMessageAsync(id);
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
            _unitOfWork.MessageRepository.DeleteMessage(message, issuerId);

            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }
            return BadRequest("Failed to delete the message");
        }
    }
}
