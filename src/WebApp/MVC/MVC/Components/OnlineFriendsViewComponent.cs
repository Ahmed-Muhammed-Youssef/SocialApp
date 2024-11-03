using Microsoft.AspNetCore.Mvc;

namespace MVC.Components
{
    public class OnlineFriendsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
