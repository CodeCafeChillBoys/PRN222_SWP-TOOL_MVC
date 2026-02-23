using PRN222_SWP_TOOL_MVC.Service.DTO.QnA;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.IQnA
{
    public interface IQnaService
    {
        // ── Student endpoints ───────────────────────────────────────────

        /// <summary>Tạo câu hỏi mới. StudentID = currentUserId.</summary>
        Task<ApiResponse<QuestionDetailDto>> CreateQuestionAsync(
            int currentUserId, CreateQuestionRequest request);

        /// <summary>Xem danh sách câu hỏi của nhóm mình.</summary>
        Task<ApiResponse<IEnumerable<QuestionListItemDto>>> GetStudentQuestionsAsync(
            int currentUserId, int groupId, string? status);

        /// <summary>Xem chi tiết + thread messages (student).</summary>
        Task<ApiResponse<QuestionDetailDto>> GetQuestionDetailForStudentAsync(
            int currentUserId, int questionId);

        /// <summary>Student gửi thêm tin nhắn. Re-open nếu ANSWERED/CLOSED.</summary>
        Task<ApiResponse<QuestionDetailDto>> AddStudentMessageAsync(
            int currentUserId, int questionId, AddMessageRequest request);

        // ── Teacher endpoints ───────────────────────────────────────────

        /// <summary>Xem danh sách câu hỏi thuộc topic mình phụ trách.</summary>
        Task<ApiResponse<IEnumerable<QuestionListItemDto>>> GetTeacherQuestionsAsync(
            int currentUserId, int topicId, string? status);

        /// <summary>Teacher cập nhật trạng thái câu hỏi (PENDING→PROCESSING→ANSWERED→CLOSED).</summary>
        Task<ApiResponse<bool>> UpdateQuestionStatusAsync(
            int currentUserId, int questionId, UpdateStatusRequest request);

        /// <summary>Teacher trả lời → auto set ANSWERED.</summary>
        Task<ApiResponse<QuestionDetailDto>> AddTeacherMessageAsync(
            int currentUserId, int questionId, AddMessageRequest request);
    }
}
