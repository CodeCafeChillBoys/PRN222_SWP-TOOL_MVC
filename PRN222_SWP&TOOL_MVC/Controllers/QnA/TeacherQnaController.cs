using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Service.DTO.QnA;
using PRN222_SWP_TOOL_MVC.Service.IServices.IQnA;
using System.Security.Claims;

namespace PRN222_SWP_TOOL_MVC.Controllers.QnA
{
    [ApiController]
    [Route("api/teacher/questions")]
    public class TeacherQnaController : ControllerBase
    {
        private readonly IQnaService _qnaService;

        public TeacherQnaController(IQnaService qnaService)
        {
            _qnaService = qnaService;
        }

        /// <summary>Lấy UserID của teacher đang đăng nhập từ JWT claims.</summary>
        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        // ── GET /api/teacher/questions?topicId=&status= ────────────────
        /// <summary>Danh sách câu hỏi thuộc topic mình phụ trách.</summary>
        [HttpGet]
        public async Task<IActionResult> GetQuestions(
            [FromQuery] int topicId,
            [FromQuery] string? status = null)
        {
            var result = await _qnaService.GetTeacherQuestionsAsync(CurrentUserId, topicId, status);
            return result.Success ? Ok(result) : MapError(result);
        }

        // ── PATCH /api/teacher/questions/{id}/status ───────────────────
        /// <summary>Cập nhật status câu hỏi (PENDING→PROCESSING→ANSWERED→CLOSED).</summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var result = await _qnaService.UpdateQuestionStatusAsync(CurrentUserId, id, request);
            return result.Success ? Ok(result) : MapError(result);
        }

        // ── POST /api/teacher/questions/{id}/messages ──────────────────
        /// <summary>Teacher trả lời câu hỏi → auto set ANSWERED.</summary>
        [HttpPost("{id}/messages")]
        public async Task<IActionResult> AddMessage(int id, [FromBody] AddMessageRequest request)
        {
            var result = await _qnaService.AddTeacherMessageAsync(CurrentUserId, id, request);
            return result.Success ? Ok(result) : MapError(result);
        }

        // ── HTTP error mapping ─────────────────────────────────────────
        private IActionResult MapError<T>(ApiResponse<T> result) => result.ReasonCode switch
        {
            ReasonCodes.NOT_TOPIC_OWNER           => StatusCode(403, result),
            ReasonCodes.QUESTION_NOT_FOUND        => NotFound(result),
            ReasonCodes.INVALID_STATUS_TRANSITION => Conflict(result),
            ReasonCodes.INVALID_STATUS            => BadRequest(result),
            _                                     => BadRequest(result)
        };
    }
}
