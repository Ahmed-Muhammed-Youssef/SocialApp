using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize]
    public class NewsfeedController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public NewsfeedController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> IndexAsync()
        {
            // to have a consistant list of posts we need to pass the time first ordered the list
            string identityId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var user = await unitOfWork.ApplicationUserRepository.GetByIdentity(identityId);
            var posts = await unitOfWork.PostRepository.GetNewsfeed(user.Id, new Application.DTOs.Pagination.PaginationParams() { ItemsPerPage = 20 });
            return View(posts);
        }
    }
}
