using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] UserParams userParams)
        {
            if (string.IsNullOrEmpty(userParams.Sex))
            {
                var interest = await _unitOfWork.UserRepository.GetUserInterest(User.GetId());
                userParams.Sex = interest.ToString();
            }
            var friendrequested = await _unitOfWork.FriendRequestRepository.GetFriendRequestedUsersDTOAsync(User.GetId());
            var forbiddenIds = await _unitOfWork.FriendRequestRepository.GetFriendsIdsAsync(User.GetId());
            forbiddenIds = forbiddenIds.Concat(friendrequested.Select(u => u.Id));
            var users = await _unitOfWork.UserRepository.GetUsersDTOAsync(User.GetUsername(), userParams, forbiddenIds.ToList());
            var newPaginationHeader = new PaginationHeader(users.CurrentPage, users.ItemsPerPage, users.TotalCount, users.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader);
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserDTOByIdAsync(id);

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
            var appUser = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetId());

            if (appUser == null)
            {
                return BadRequest(userDTO);
            }
            _mapper.Map(userDTO, appUser);
            _unitOfWork.UserRepository.Update(appUser);

            if (await _unitOfWork.Complete())
            {
                return Ok(userDTO);
            }
            return BadRequest(userDTO);
        }
    }
}
