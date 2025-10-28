using Application.Features.Users;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Shared.Extensions;

namespace MVC.Controllers;

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
        int publicId = User.GetPublicId();

        var thread = await unitOfWork.MessageRepository.GetMessagesDTOThreadAsync(publicId, userId);

        var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        ChatViewModel chatDTO = new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePictureUrl = user.ProfilePictureUrl,
            MessagesThread = thread
        };

        return PartialView("_ChatPartial", chatDTO);
    }
}
