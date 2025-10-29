using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Shared.Extensions;

namespace MVC.Controllers;

public class ProfileController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfileController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
    {
        var publicId = User.GetPublicId();

        var userProfile = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id, cancellationToken);

        if(userProfile is null)
        {
            return NotFound();
        }

        UserProfileViewModel profile = new()
        { 
            Id = id,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            LastActive = userProfile.LastActive,
            Age = userProfile.DateOfBirth.CalculateAge(),
            Bio = userProfile.Bio,
            Sex = userProfile.Sex,
            Created = userProfile.Created,
            ProfilePictureUrl = userProfile.ProfilePictureUrl,
            Relation = SocialRelation.NotFriend
        };

        bool isFriend = await _unitOfWork.FriendRequestRepository.IsFriend(publicId, id);

        if (isFriend) {
            profile.Relation = SocialRelation.Friend;
            return View(profile);
        }

        bool sentFriendRequest = await _unitOfWork.FriendRequestRepository.IsFriendRequestedAsync(publicId, id);

        if (sentFriendRequest) 
        { 
            profile.Relation = SocialRelation.FrinedRequesSent;
            return View(profile);
        }

        bool receivedFriendRequest = await _unitOfWork.FriendRequestRepository.IsFriendRequestedAsync(publicId, id);

        if (receivedFriendRequest) 
        {
            profile.Relation = SocialRelation.FrinedRequesSent;
            return View(profile);
        }

        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> SendFriendRequest(int id, CancellationToken cancellationToken)
    {
        // retuns true if the user has become a frined
        var sender = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(User.GetPublicId(), cancellationToken);
        var target = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id, cancellationToken);
        if (sender == null || target == null)
        {
            return NotFound();
        }
        if (sender.Id == target.Id)
        {
            return BadRequest("You can't send friend requests to yourself.");
        }
        if (await _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(sender.Id, target.Id) != null)
        {
            return BadRequest("You already sent a frient request to this user.");
        }
        if (await _unitOfWork.FriendRequestRepository.IsFriend(sender.Id, target.Id) == true)
        {
            return BadRequest("You already are friends.");
        }
        bool isFriend = await _unitOfWork.FriendRequestRepository.SendFriendRequest(sender.Id, target.Id);
        
        await _unitOfWork.SaveChangesAsync();

        return Ok(isFriend);
    }

    [HttpPost]
    public async Task<IActionResult> UnsendFriendRequest(int id, CancellationToken cancellationToken)
    {
        // retuns true if the user has become a frined
        int? senderId = User.GetPublicId();

        var targetId = id;

        if (senderId is null)
        {
            return BadRequest();
        }

        FriendRequest? fr = await _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(senderId.Value, targetId);

        if (fr is null)
        {
            return BadRequest("Invalid Operation");
        }
       
        await _unitOfWork.FriendRequestRepository.DeleteAsync(fr, cancellationToken);

        return NoContent();
    }
}
