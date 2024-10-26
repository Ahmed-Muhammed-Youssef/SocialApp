using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            Application.DTOs.User.UserDTO userProfile = await _unitOfWork.ApplicationUserRepository.GetDtoByIdAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);
        }
    }
}
