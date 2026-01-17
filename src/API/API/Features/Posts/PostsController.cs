namespace API.Features.Posts;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController(IMediator mediator, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedList<PostDTO>>> Get([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        Result<PagedList<PostDTO>> result = await mediator.Send(new GetPostsQuery(paginationParams), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            var pd = problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status400BadRequest, title: "Failed to get posts", detail: string.Join("; ", result.Errors));
            return BadRequest(pd);
        }
    }

    [HttpGet("{postId:long}")]
    public async Task<ActionResult<Post>> GetById(ulong postId, CancellationToken cancellationToken)
    {
        Result<Post> result = await mediator.Send(new GetPostByIdQuery(postId), cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            var pd = problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status404NotFound, title: "Post not found", detail: string.Join("; ", result.Errors));
            return NotFound(pd);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePostRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var pd = problemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState, StatusCodes.Status400BadRequest);
            return BadRequest(pd);
        }

        Result<ulong> result = await mediator.Send(new CreatePostCommand(request.Content), cancellationToken);
        if (result.IsSuccess)
        {
            // Use Url.Action to manually construct the location instead of CreatedAtAction
            var location = Url.Action(nameof(GetById), "Posts", new { postId = result.Value });
            return Created(location, null);
        }
        else
        {
            var pd = problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status400BadRequest, title: "Create post failed", detail: string.Join("; ", result.Errors));
            return BadRequest(pd);
        }
    }
}
