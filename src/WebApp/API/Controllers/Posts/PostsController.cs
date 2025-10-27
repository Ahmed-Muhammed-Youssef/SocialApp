using API.Controllers.Posts.Requests;

namespace API.Controllers.Posts;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetUserPostsAsync([FromQuery]int userId)
    {
        var result = await mediator.Send(new GetPostsByOwnerIdQuery(userId));

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
    public async Task<ActionResult<Post>> GetPostById(ulong postId)
    {
        Result<Post> result = await mediator.Send(new GetPostByIdQuery(postId));

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
    public async Task<ActionResult> Create([FromBody]CreatePostRequest request)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Result<ulong> result = await mediator.Send(new CreatePostCommand(request.Content));
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
