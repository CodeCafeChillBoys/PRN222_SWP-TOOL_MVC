using Microsoft.AspNetCore.Mvc;

namespace PRN222_MVC.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
