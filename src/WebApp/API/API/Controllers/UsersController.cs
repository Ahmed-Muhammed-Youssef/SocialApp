﻿namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UsersController(IUnitOfWork _unitOfWork, IMapper _mapper, JsonSerializerOptions jsonSerializerOptions) : ControllerBase
    {

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] UserParams userParams)
        {
            var users = await _unitOfWork.ApplicationUserRepository.GetUsersDTOAsync(User.GetPublicId().Value, userParams);
            var newPaginationHeader = new PaginationHeader(users.CurrentPage, users.ItemsPerPage, users.Count, users.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader, jsonSerializerOptions);
            return Ok(users.Items);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetDtoByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/users/update
        [HttpPut]
        public async Task<ActionResult<UpdatedUserDTO>> PutUser(UpdatedUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(userDTO);
            }
            var appUser = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId().Value);

            if (appUser == null)
            {
                return BadRequest(userDTO);
            }
            _mapper.Map(userDTO, appUser);
            _unitOfWork.ApplicationUserRepository.Update(appUser);

            await _unitOfWork.SaveChangesAsync();

            return Ok(userDTO);
        }
    }
}
