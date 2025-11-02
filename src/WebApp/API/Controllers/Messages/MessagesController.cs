namespace API.Controllers.Messages;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessagesController(IMediator mediator) : ControllerBase
{
    // DELETE: api/Messages/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id, CancellationToken cancellationToken)
    {
        var result =  await mediator.Send(new MessageDeleteCommand(id), cancellationToken);

        if(result.IsSuccess)
        {
            return NoContent();
        }
        else if (result.Status == ResultStatus.NotFound)
        {
            return NotFound();
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
