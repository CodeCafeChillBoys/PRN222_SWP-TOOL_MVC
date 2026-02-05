using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Service.IServices.ISemester;

namespace PRN222_MVC.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ISemesterService _semesterService;

        public TeacherController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        public async Task<IActionResult> Index(int? semesterId, string? status)
        {
            var allSemesters = await _semesterService.GetAllAsync();
            
            // Filter nếu có semesterId
            var filteredSemesters = allSemesters;
            
            if (semesterId.HasValue)
            {
                filteredSemesters = filteredSemesters.Where(s => s.SemesterID == semesterId.Value).ToList();
            }
            
            // Filter theo status nếu có
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "pending")
                {
                    // Tạm thời lọc các semester chưa kết thúc
                    filteredSemesters = filteredSemesters.Where(s => s.EndDate > DateTime.Now).ToList();
                }
                else if (status == "replied")
                {
                    // Tạm thời lọc các semester đã kết thúc
                    filteredSemesters = filteredSemesters.Where(s => s.EndDate <= DateTime.Now).ToList();
                }
            }
            
            ViewBag.AllSemesters = allSemesters;
            ViewBag.SelectedSemesterId = semesterId;
            ViewBag.SelectedStatus = status;
            return View(filteredSemesters);
        }
    }
}
