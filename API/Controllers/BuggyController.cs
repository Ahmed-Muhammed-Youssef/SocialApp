using API.Data;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]


    public class BuggyController : ControllerBase
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        // GET: api/buggy/auth
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        // GET: api/buggy/not-found
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var search = _context.Users.Find(-1);
            if(search == null)
            {
                return NotFound();
            }
            return Ok(search);
        }
        
        // GET: api/buggy/server-error
        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError()
        {
            var thing = _context.Users.Find(-1);
            var toPrint = thing.ToString();
            return Ok(toPrint);
        } 

        // GET: api/buggy/bad-request
        [HttpGet("bad-request")]
        public  ActionResult<string> GetBadRequest()
        {
            return BadRequest("a bad request!");
        }
    }
}
