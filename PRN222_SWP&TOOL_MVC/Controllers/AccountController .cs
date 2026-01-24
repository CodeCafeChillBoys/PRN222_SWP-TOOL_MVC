using Microsoft.AspNetCore.Mvc;

namespace PRN222_MVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
