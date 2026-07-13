namespace API.Features.DirectChats;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController(IMediator mediator, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>
    /// Gets a paginated list of direct chats for the current user.
    /// </summary>
    /// <param name="paginationParams">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of direct chats.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<DirectChatDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Gets a direct chat by ID.
    /// </summary>
    /// <param name="id">The chat ID.</param>
    /// <returns>The direct chat details.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DirectChatDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DirectChatDTO> GetById(int id)
    {
        throw new NotImplementedException();
    }
}
