namespace API.Features.Posts;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetUserPostsAsync([FromQuery]int userId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPostsByOwnerIdQuery(userId), cancellationToken);

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
    public async Task<ActionResult<Post>> GetPostById(ulong postId, CancellationToken cancellationToken)
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
            return CreatedAtAction(nameof(GetPostById), new { postId = result.Value});
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
