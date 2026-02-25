using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Service.DTO.QnA;
using PRN222_SWP_TOOL_MVC.Service.IServices.IQnA;
using System.Security.Claims;

namespace PRN222_SWP_TOOL_MVC.Controllers.QnA
{
    [Authorize(Roles = "Teacher")]
    public class TeacherQnaController : Controller
    {
        private readonly IQnaService _qnaService;

        public TeacherQnaController(IQnaService qnaService)
        {
            _qnaService = qnaService;
        }

        private int GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                throw new InvalidOperationException("User is not authenticated");
            return int.Parse(userIdStr);
        }

        // GET: /TeacherQna/Index?topicId=&status=
        /// <summary>Danh sách câu hỏi thuộc topic mình phụ trách.</summary>
        [HttpGet]
        public async Task<IActionResult> Index(int topicId, string? status = null)
        {
            var result = await _qnaService.GetTeacherQuestionsAsync(GetCurrentUserId(), topicId, status);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index", "Teacher");
            }
            return View(result.Data);
        }

        // GET: /TeacherQna/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _qnaService.GetQuestionDetailForTeacherAsync(GetCurrentUserId(), id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index", "Teacher");
            }
            return View(result.Data);
        }

        // POST: /TeacherQna/UpdateStatus/5
        /// <summary>Cập nhật status câu hỏi (PENDING → PROCESSING → ANSWERED → CLOSED).</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusRequest request)
        {
            var result = await _qnaService.UpdateQuestionStatusAsync(GetCurrentUserId(), id, request);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Detail", new { id });
        }

        // POST: /TeacherQna/AddMessage/5
        /// <summary>Teacher trả lời câu hỏi → auto set ANSWERED.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessage(int id, AddMessageRequest request)
        {
            var result = await _qnaService.AddTeacherMessageAsync(GetCurrentUserId(), id, request);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Detail", new { id });
        }
    }
}
