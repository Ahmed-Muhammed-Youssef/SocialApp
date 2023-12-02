using API.Application.DTOs;
using API.Application.Interfaces;
using API.Extensions;
using API.Filters;
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
            var users = await _unitOfWork.UserRepository.GetUsersDTOAsync(User.GetId(), userParams);
            var newPaginationHeader = new PaginationHeader(users.CurrentPage, users.ItemsPerPage, users.Count, users.TotalPages);
            Response.AddPaginationHeader(newPaginationHeader);
            return Ok(users.Items);
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
