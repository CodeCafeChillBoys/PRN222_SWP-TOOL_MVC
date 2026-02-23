using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Service.DTO.QnA;
using PRN222_SWP_TOOL_MVC.Service.IServices.IQnA;
using System.Security.Claims;

namespace PRN222_SWP_TOOL_MVC.Controllers.QnA
{
    [ApiController]
    [Route("api/student/questions")]
    public class StudentQnaController : ControllerBase
    {
        private readonly IQnaService _qnaService;

        public StudentQnaController(IQnaService qnaService)
        {
            _qnaService = qnaService;
        }

        /// <summary>Lấy UserID của student đang đăng nhập từ JWT claims.</summary>
        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        // ── POST /api/student/questions ────────────────────────────────
        /// <summary>Tạo câu hỏi mới. Body: { groupId, topicId, content, title? }</summary>
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            var result = await _qnaService.CreateQuestionAsync(CurrentUserId, request);
            return result.Success ? StatusCode(201, result) : MapError(result);
        }

        // ── GET /api/student/questions?groupId=&status= ────────────────
        /// <summary>Danh sách câu hỏi của nhóm mình.</summary>
        [HttpGet]
        public async Task<IActionResult> GetQuestions(
            [FromQuery] int groupId,
            [FromQuery] string? status = null)
        {
            var result = await _qnaService.GetStudentQuestionsAsync(CurrentUserId, groupId, status);
            return result.Success ? Ok(result) : MapError(result);
        }

        // ── GET /api/student/questions/{id} ───────────────────────────
        /// <summary>Chi tiết câu hỏi + thread messages.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion(int id)
        {
            var result = await _qnaService.GetQuestionDetailForStudentAsync(CurrentUserId, id);
            return result.Success ? Ok(result) : MapError(result);
        }

        // ── POST /api/student/questions/{id}/messages ──────────────────
        /// <summary>Gửi thêm tin nhắn vào thread.</summary>
        [HttpPost("{id}/messages")]
        public async Task<IActionResult> AddMessage(int id, [FromBody] AddMessageRequest request)
        {
            var result = await _qnaService.AddStudentMessageAsync(CurrentUserId, id, request);
            return result.Success ? Ok(result) : MapError(result);
        }

        // ── HTTP error mapping ─────────────────────────────────────────
        private IActionResult MapError<T>(ApiResponse<T> result) => result.ReasonCode switch
        {
            ReasonCodes.NOT_GROUP_MEMBER    => StatusCode(403, result),
            ReasonCodes.QUESTION_NOT_FOUND  => NotFound(result),
            ReasonCodes.GROUP_NO_TOPIC      => UnprocessableEntity(result),
            _                               => BadRequest(result)
        };
    }
}
