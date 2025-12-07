namespace API.Features.Posts;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedList<PostDTO>>> Get([FromQuery]PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        Result<PagedList<PostDTO>> result = await mediator.Send(new GetPostsQuery(paginationParams), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<Post>> GetById(ulong postId, CancellationToken cancellationToken)
    {
        Result<Post> result = await mediator.Send(new GetPostByIdQuery(postId), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound(result.Errors);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody]CreatePostRequest request, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Result<ulong> result = await mediator.Send(new CreatePostCommand(request.Content), cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetById), new { postId = result.Value});
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
