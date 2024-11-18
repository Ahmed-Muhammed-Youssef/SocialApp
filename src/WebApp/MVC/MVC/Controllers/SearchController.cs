using Application.DTOs.Pagination;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;

namespace MVC.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> UsersAsync(string search, UserParams userParams)
        {
            int? publicId = User.GetPublicId();
            if (publicId is not null)
            {
                var users = await _unitOfWork.ApplicationUserRepository.SearchAsync(publicId.Value, search, userParams);
                return View(users);
            }

            return View();
        }
    }
}
