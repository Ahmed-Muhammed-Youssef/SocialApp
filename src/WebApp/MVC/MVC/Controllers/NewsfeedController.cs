using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class NewsfeedController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public NewsfeedController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> IndexAsync()
        {
            // problems to fix:
            // validate user id
            // to have a consistant list of posts we need to pass the time first ordered the list

            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id))
            {
                var posts = await unitOfWork.PostRepository.GetNewsfeed(id, new Application.DTOs.Pagination.PaginationParams());
                return View(posts);
            }

            return RedirectToAction("Index", "HomeController");
        }
    }
}
