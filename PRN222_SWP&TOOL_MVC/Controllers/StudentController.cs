using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using System.Security.Claims;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{

    public StudentController()
    {
    }
    public async Task<IActionResult> Index(string tab = "group")
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var request = new StudentDashboardFilterRequestDTO
        {
            UserId = userId,
            Tab = tab
        };
        return View();
    }


}