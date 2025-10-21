namespace API.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController(IUnitOfWork _unitOfWork, IMapper _mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetUserPostsAsync([FromQuery]int userId)
    {
        var posts = await _unitOfWork.PostRepository.GetUserPostsAsync(userId, User.GetPublicId());
        return Ok(posts);
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostByIdAsync(ulong postId)
    {
        var posts = await _unitOfWork.PostRepository.GetByIdAsync(postId, User.GetPublicId());
        return Ok(posts);
    }

    [HttpPost]
    public async Task<ActionResult<PostDTO>> AddPost([FromBody]AddPostDTO newPostDTO)
    {
        Post post = _mapper.Map<Post>(newPostDTO);
        post.UserId = User.GetPublicId();

        await _unitOfWork.PostRepository.AddAsync(post);

        await _unitOfWork.SaveChangesAsync();

        PostDTO returnPostDTO = _mapper.Map<PostDTO>(post);
        returnPostDTO.Id = post.Id;

        return Ok(returnPostDTO);
    }
}
