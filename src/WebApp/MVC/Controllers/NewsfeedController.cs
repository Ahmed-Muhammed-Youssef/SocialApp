using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using Shared.Pagination;

namespace MVC.Controllers;

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
        int? publicId = User.GetPublicId();
        if (publicId is not null)
        {
            var posts = await unitOfWork.PostRepository.GetNewsfeed(publicId.Value, new PaginationParams() { ItemsPerPage = 20 });
            return View(posts);
        }

        return BadRequest("Error accessing user ID");
    }

    public async Task<IActionResult> LoadPosts(int pageNumber)
    {
        int? publicId = User.GetPublicId();

        if(publicId is not null)
        {
            var posts = await unitOfWork.PostRepository.GetNewsfeed(publicId.Value, new PaginationParams() { ItemsPerPage = 20, PageNumber = pageNumber });

            if (posts.Count > 0) 
            { 
                return PartialView("_PostsListPartial", posts.Items);
            }

            Response.Headers.Append("x-no-posts", "true");

            return PartialView("_NoPostsPartial");
        }

        return BadRequest("Error accessing user ID");
    }
}
