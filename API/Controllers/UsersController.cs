using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.Extensions;
using API.Helpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/users/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] UserParams userParams)
        {
            if(string.IsNullOrEmpty(userParams.Sex))
            {
                var interest = await _unitOfWork.UsersRepository.GetUserInterest(User.GetId());
                userParams.Sex = interest.ToString();
            }
            var forbiddenIds = await _unitOfWork.FriendRequestsRepository.GetFriendRequestedUsersIdAsync(User.GetId());
            var users = await _unitOfWork.UsersRepository.GetUsersDTOAsync(User.GetUsername() ,userParams, forbiddenIds.ToList());
            var newPaginationHeader = new PaginationHeader(users.CurrentPage, users.ItemsPerPage, users.TotalCount, users.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader);
            return Ok(users);
        }

        // GET: api/users/info/{username}
        [HttpGet("info/{username}")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            var user = await _unitOfWork.UsersRepository.GetUserDTOByUsernameAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/users/update
        [HttpPut("update")]
        public async Task<ActionResult<UpdatedUserDTO>> PutUser(UpdatedUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(userDTO);
            }
            var appUser = await _unitOfWork.UsersRepository.GetUserByUsernameAsync(User.GetUsername());

            if (appUser == null)
            {
                return BadRequest(userDTO);
            }
            _mapper.Map(userDTO, appUser);
            _unitOfWork.UsersRepository.Update(appUser);

            if(await _unitOfWork.Complete())
            {
                return Ok(userDTO);
            }
            return BadRequest(userDTO);
        }
    }
}
