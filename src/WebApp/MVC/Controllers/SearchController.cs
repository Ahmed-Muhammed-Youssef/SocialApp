using Application.Common.Interfaces;
using Application.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;

namespace MVC.Controllers;

[Authorize]
public class SearchController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsersAsync(string search)
    {
        int? publicId = User.GetPublicId();
        if (publicId is not null)
        {
            var users = await _unitOfWork.ApplicationUserRepository.SearchAsync(publicId.Value, search, new UserParams());
            return View(users);
        }

        return BadRequest();
    }
}
