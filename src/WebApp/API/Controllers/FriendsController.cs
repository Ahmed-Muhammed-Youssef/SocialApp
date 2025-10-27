namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ServiceFilter(typeof(LogUserActivity))]

public class FriendsController(IUnitOfWork _unitOfWork) : ControllerBase
{
    // GET: api/friends/isfriend/{id}
    [HttpGet("isFriend/{id}")]
    public async Task<ActionResult<bool>> IsFriend(int id)
    {
        return Ok(await _unitOfWork.FriendRequestRepository.IsFriend(User.GetPublicId(), id));
    }
}
