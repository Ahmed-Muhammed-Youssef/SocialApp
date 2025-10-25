namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ServiceFilter(typeof(LogUserActivity))]

public class FriendsController(IUnitOfWork _unitOfWork, JsonSerializerOptions jsonSerializerOptions) : ControllerBase
{
    // GET: api/friends
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId());
        if (user == null)
        {
            return Unauthorized();
        }
        var friends = await _unitOfWork.FriendRequestRepository.GetFriendsAsync(user.Id, paginationParams);
        var newPaginationHeader = new PaginationHeader(friends.CurrentPage, friends.ItemsPerPage, friends.Count, friends.TotalPages);
        Response.AddPaginationHeader(newPaginationHeader, jsonSerializerOptions);

        return Ok(friends.Items);
    }

    // GET: api/friends/isfriend/{id}
    [HttpGet("isFriend/{id}")]
    public async Task<ActionResult<bool>> IsFriend(int id)
    {
        return Ok(await _unitOfWork.FriendRequestRepository.IsFriend(User.GetPublicId(), id));
    }
}
