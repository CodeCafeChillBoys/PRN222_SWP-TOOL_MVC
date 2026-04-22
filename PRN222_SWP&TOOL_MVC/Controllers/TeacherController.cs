using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Models;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using PRN222_SWP_TOOL_MVC.Service.IServices.ISemester;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITeacher;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITopic;
using System.Security.Claims;

namespace PRN222_MVC.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly ISemesterService _semesterService;
        private readonly ITeacherService _teacherService;
        private readonly ITopicService _topicService;

        public TeacherController(ISemesterService semesterService, ITeacherService teacherService, ITopicService topicService)
        {
            _semesterService = semesterService;
            _teacherService = teacherService;
            _topicService = topicService;
        }

        public async Task<IActionResult> Semester(int? semesterId, string? status)
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

            var model = allSemesters.Select(s => new SemesterDashboardViewModel
            {
                semesterResponseDTO = s
            }).ToList();

            ViewBag.AllSemesters = allSemesters;
            ViewBag.SelectedSemesterId = semesterId;
            ViewBag.SelectedStatus = status;
            return View(model);
        }
        public async Task<IActionResult> Index(string tab = "qa")
        {
            var dto = await _teacherService.GetDashboardDataAsync(tab);

            var model = new StudentDashboardViewModel
            {
                CurrentTab = tab,
                teacherDashboardResDTO = dto
            };

            return View(model);
        }
        public async Task<IActionResult> CreateTopic()
        {
            var semesters = await _semesterService.GetAllAsync();

            var model = semesters.Select(s => new SemesterDashboardViewModel
            {
                semesterResponseDTO = s
            }).ToList();

            return PartialView(
                "~/Views/Teacher/ComponentTeacher/_CreateTopic.cshtml",
                model
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetTopic(int id)
        {
            var result = await _topicService.GetTopicDetailAsync(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTopicRequestDTO request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, message = "Chưa đăng nhập" });
                }

                var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (teacherIdClaim == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin giáo viên" });
                }

                int teacherId = int.Parse(teacherIdClaim.Value);

                var result = await _topicService.CreateTopicAsync(request, teacherId);

                return Json(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTopic(UpdateTopicRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false });

            var result = await _topicService.UpdateAsync(dto);

            return Json(new { success = result });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var result = await _topicService.DeleteTopic(id);
            return Json(new { success = result });
        }
    }
}
