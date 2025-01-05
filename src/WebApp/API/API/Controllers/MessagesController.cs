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
            var issuerId = User.GetPublicId().Value;

            // Checks if the message is related to the issuer
            if (message.SenderId != issuerId && message.RecipientId != issuerId)
            {
                return BadRequest("Failed to delete the message");
            }
            _unitOfWork.MessageRepository.DeleteMessage(message, issuerId);

            if (await _unitOfWork.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to delete the message");
        }
    }
}
