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
        private readonly DataContext context;

        public BuggyController(DataContext context)
        {
            this.context = context;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }
        [HttpGet("not-foud")]
        public ActionResult<AppUser> GetNotFound()
        {
            var search = context.Users.Find(-1);
            if(search == null)
            {
                return NotFound();
            }
            return Ok(search);
        } 
        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError()
        {
            var thing = context.Users.Find(-1);
            var toPrint = thing.ToString();
            return Ok(toPrint);
        } 
        [HttpGet("bad-request")]
        public  ActionResult<string> GetBadRequest()
        {
            return BadRequest("a bad request!");
        }
    }
}
