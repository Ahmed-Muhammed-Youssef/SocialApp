using Application.DTOs.User;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;

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
            var publicId = User.GetPublicId();

            if (publicId == null)
            {
                return NotFound();
            }

            var userProfile = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id);

            if(userProfile is null)
            {
                return NotFound();
            }

            UserProfileDTO profile = new()
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

            bool isFriend = await _unitOfWork.FriendRequestRepository.IsFriend(publicId.Value, id);

            if (isFriend) {
                profile.Relation = SocialRelation.Friend;
                return View(profile);
            }

            bool sentFriendRequest = await _unitOfWork.FriendRequestRepository.IsFriendRequestedAsync(publicId.Value, id);

            if (sentFriendRequest) 
            { 
                profile.Relation = SocialRelation.FrinedRequesSent;
                return View(profile);
            }

            bool receivedFriendRequest = await _unitOfWork.FriendRequestRepository.IsFriendRequestedAsync(publicId.Value, id);

            if (receivedFriendRequest) 
            {
                profile.Relation = SocialRelation.FrinedRequesSent;
                return View(profile);
            }

            return View(profile);
        }
    }
}
