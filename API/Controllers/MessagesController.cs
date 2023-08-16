using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public MessagesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // DELETE: api/Messages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await unitOfWork.MessagesRepository.GetMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            var issuerId = User.GetId();
            // IMPORTANT
            //@ToDo Check if the message is related to the issuer
            unitOfWork.MessagesRepository.DeleteMessage(message, issuerId);

            if (await unitOfWork.Complete()) { 
                return NoContent();
            }
            return BadRequest("Failed to delete the message");
        }
    }
}
