namespace API.Features.Posts;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController(IMediator mediator, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>
    /// Gets a paginated list of posts.
    /// </summary>
    /// <param name="paginationParams">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of posts.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<PostDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Gets a post by its ID.
    /// </summary>
    /// <param name="postId">The post ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The post details.</returns>
    [HttpGet("{postId:long}")]
    [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="request">The post creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success status with the created post location.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
