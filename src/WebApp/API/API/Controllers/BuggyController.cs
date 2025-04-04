﻿namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class BuggyController(DataContext _context) : ControllerBase
    {
        // GET: api/buggy/auth
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        // GET: api/buggy/not-found
        [HttpGet("not-found")]
        public ActionResult<ApplicationUser> GetNotFound()
        {
            var search = _context.ApplicationUsers.Find(-1);
            if (search == null)
            {
                return NotFound();
            }
            return Ok(search);
        }

        // GET: api/buggy/server-error
        [HttpGet("server-error")]
        public ActionResult<ApplicationUser> GetServerError()
        {
            var thing = _context.ApplicationUsers.Find(-1);
            var toPrint = thing.ToString();
            return Ok(toPrint);
        }

        // GET: api/buggy/bad-request
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("a bad request!");
        }
    }
}
