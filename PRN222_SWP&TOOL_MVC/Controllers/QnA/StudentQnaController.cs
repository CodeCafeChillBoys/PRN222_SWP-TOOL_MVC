using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Service.DTO.QnA;
using PRN222_SWP_TOOL_MVC.Service.IServices.IQnA;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITopic;
using System.Security.Claims;

namespace PRN222_SWP_TOOL_MVC.Controllers.QnA
{
    [Authorize(Roles = "Student")]
    public class StudentQnaController : Controller
    {
        private readonly IQnaService _qnaService;
        private readonly ITopicService _topicService;

        public StudentQnaController(IQnaService qnaService, ITopicService topicService)
        {
            _qnaService   = qnaService;
            _topicService = topicService;
        }

        private int GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                throw new InvalidOperationException("User is not authenticated");
            return int.Parse(userIdStr);
        }

        // GET: /StudentQna/Index?groupId=&status=
        /// <summary>Danh sách câu hỏi của nhóm mình.</summary>
        [HttpGet]
        public async Task<IActionResult> Index(int groupId, string? status = null)
        {
            var result = await _qnaService.GetStudentQuestionsAsync(GetCurrentUserId(), groupId, status);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index", "Student");
            }
            return View(result.Data);
        }

        // GET: /StudentQna/Detail/5
        /// <summary>Chi tiết câu hỏi + thread messages.</summary>
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _qnaService.GetQuestionDetailForStudentAsync(GetCurrentUserId(), id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index", "Student");
            }
            return View(result.Data);
        }

        // GET: /StudentQna/Create?groupId=&topicId=
        [HttpGet]
        public async Task<IActionResult> Create(int groupId, int topicId = 0)
        {
            // Chỉ truyền topic đã đăng ký (nếu có) vào ViewBag
            if (topicId > 0)
            {
                var topic = await _topicService.GetByIdAsync(topicId);
                ViewBag.Topics = topic != null
                    ? new List<PRN222_SWP_TOOL_MVC.Repository.Entities.Topic> { topic }
                    : new List<PRN222_SWP_TOOL_MVC.Repository.Entities.Topic>();
            }
            else
            {
                ViewBag.Topics = await _topicService.GetAllTopicsAsync();
            }
            return View(new CreateQuestionRequest(groupId, topicId, string.Empty));
        }

        // POST: /StudentQna/Create
        /// <summary>Tạo câu hỏi mới.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuestionRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Topics = await _topicService.GetAllTopicsAsync();
                return View(request);
            }

            var result = await _qnaService.CreateQuestionAsync(GetCurrentUserId(), request);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                ViewBag.Topics = await _topicService.GetAllTopicsAsync();
                return View(request);
            }

            TempData["Success"] = "Câu hỏi đã được gửi thành công.";
            return RedirectToAction("Index", new { groupId = request.GroupId });
        }

        // POST: /StudentQna/AddMessage/5
        /// <summary>Gửi thêm tin nhắn vào thread.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessage(int id, AddMessageRequest request)
        {
            var result = await _qnaService.AddStudentMessageAsync(GetCurrentUserId(), id, request);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Detail", new { id });
        }
    }
}
