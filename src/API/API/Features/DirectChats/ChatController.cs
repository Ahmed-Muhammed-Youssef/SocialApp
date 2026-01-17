namespace API.Features.DirectChats;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController(IMediator mediator, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    // GET: api/Chat
    [HttpGet]
    public async Task<ActionResult<PagedList<DirectChatDTO>>> GetAsync([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        Result<PagedList<DirectChatDTO>> result = await mediator.Send(new GetChatsQuery(paginationParams), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            var pd = problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status400BadRequest, title: "Failed to get chats", detail: string.Join("; ", result.Errors));
            return BadRequest(pd);
        }
    }

    // GET: api/chat/id
    [HttpGet("{id:int}")]
    public ActionResult GetById(int id)
    {
        throw new NotImplementedException();
    }
}
