using Application.DTOs.Message;
using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;

namespace MVC.Controllers
{
    [Authorize]
    public class MessengerController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public MessengerController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> IndexAsync()
        {
            int? publicId = User.GetPublicId();

            if (publicId is not null)
            {
                List<SimplifiedUserDTO> inbox = await unitOfWork.MessageRepository.GetInboxAsync(publicId.Value);
                return View(inbox);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> LoadChat(int userId)
        {
            int? publicId = User.GetPublicId();
            if(publicId is not null)
            {
                var thread = await unitOfWork.MessageRepository.GetMessagesDTOThreadAsync(publicId.Value, userId);

                var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);

                ChatDTO chatDTO = new ChatDTO()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    MessagesThread = thread
                };

                return PartialView("_ChatPartial", chatDTO);
            }

            return BadRequest();
        }
    }
}
