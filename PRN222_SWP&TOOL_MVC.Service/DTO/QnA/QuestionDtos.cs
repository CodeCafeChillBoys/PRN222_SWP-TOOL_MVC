namespace PRN222_SWP_TOOL_MVC.Service.DTO.QnA
{
    // ══════════════════════════════════════════════════
    // Standard API response wrapper
    // ══════════════════════════════════════════════════
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public string? ReasonCode { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }

        public static ApiResponse<T> Ok(T data, string message = "Thành công")
            => new() { Success = true, Message = message, Data = data };

        public static ApiResponse<T> Fail(string reasonCode, string message)
            => new() { Success = false, ReasonCode = reasonCode, Message = message };
    }

    // ══════════════════════════════════════════════════
    // Business reason codes — dùng để map HTTP status
    // ══════════════════════════════════════════════════
    public static class ReasonCodes
    {
        public const string NOT_GROUP_MEMBER        = "NOT_GROUP_MEMBER";
        public const string GROUP_NO_TOPIC          = "GROUP_NO_TOPIC";
        public const string NOT_TOPIC_OWNER         = "NOT_TOPIC_OWNER";
        public const string QUESTION_NOT_FOUND      = "QUESTION_NOT_FOUND";
        public const string INVALID_STATUS          = "INVALID_STATUS";
        public const string INVALID_STATUS_TRANSITION = "INVALID_STATUS_TRANSITION";
        public const string QUESTION_CLOSED         = "QUESTION_CLOSED";
    }

    // ══════════════════════════════════════════════════
    // Request DTOs
    // ══════════════════════════════════════════════════
    public record CreateQuestionRequest(
        int GroupId,
        int TopicId,
        string Content,
        string? Title = null
    );

    public record AddMessageRequest(string Content);

    public record UpdateStatusRequest(string Status);

    // ══════════════════════════════════════════════════
    // Response DTOs
    // ══════════════════════════════════════════════════
    public record MessageDto(
        int Id,
        string SenderName,
        string SenderRole,   // "STUDENT" | "TEACHER"
        string Content,
        DateTime CreatedAt
    );

    public record QuestionListItemDto(
        int Id,
        string? Title,
        string Content,
        string Status,
        string StudentName,
        string TopicName,
        DateTime CreatedAt,
        DateTime LastReplyAt,
        int MessageCount
    );

    public record QuestionDetailDto(
        int Id,
        string? Title,
        string Content,
        string Status,
        string StudentName,
        string TopicName,
        string GroupName,
        DateTime CreatedAt,
        IEnumerable<MessageDto> Messages
    );
}
