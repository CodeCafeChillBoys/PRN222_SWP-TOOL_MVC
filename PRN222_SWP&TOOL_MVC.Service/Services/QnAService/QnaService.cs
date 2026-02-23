using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.Enums;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.QnA;
using PRN222_SWP_TOOL_MVC.Service.IServices.IQnA;

namespace PRN222_SWP_TOOL_MVC.Service.Services.QnAService
{
    public class QnaService : IQnaService
    {
        private readonly IUnitOfWork _uow;

        // Thứ tự status hợp lệ — chỉ được đi tới (forward-only)
        private static readonly string[] StatusOrder =
            ["PENDING", "PROCESSING", "ANSWERED", "CLOSED"];

        public QnaService(IUnitOfWork uow) => _uow = uow;

        // ════════════════════════════════════════════════════════════════
        // STUDENT — Tạo câu hỏi mới
        // ════════════════════════════════════════════════════════════════
        public async Task<ApiResponse<QuestionDetailDto>> CreateQuestionAsync(
            int currentUserId, CreateQuestionRequest req)
        {
            // 1. Kiểm tra group membership (GroupMember.StudentID = User.UserID)
            var isMember = await _uow.groupMemberRepository.IsMemberAsync(req.GroupId, currentUserId);
            if (!isMember)
                return ApiResponse<QuestionDetailDto>.Fail(
                    ReasonCodes.NOT_GROUP_MEMBER, "Bạn không thuộc nhóm này.");

            // 2. Kiểm tra topic có tồn tại không
            var topic = await _uow.topicRepository.GetByIdAsync(req.TopicId);
            if (topic == null)
                return ApiResponse<QuestionDetailDto>.Fail(
                    ReasonCodes.GROUP_NO_TOPIC, "Topic không tồn tại.");

            // 3. Tạo Question
            var now = DateTime.UtcNow;
            var question = new Question
            {
                Title               = req.Title,
                Content             = req.Content,
                TopicID             = req.TopicId,
                GroupID             = req.GroupId,
                StudentID           = currentUserId,
                Status              = "PENDING",
                CreatedAt           = now,
                UpdatedAt           = now,
                LastReplyAt         = now,
                LastRepliedByUserId = currentUserId
            };
            await _uow.questionRepository.AddAsync(question);
            await _uow.SaveChangeAsync();   // flush để có question.Id

            // 4. Thêm message đầu tiên
            question.Messages.Add(new QuestionMessage
            {
                QuestionId   = question.Id,
                SenderUserId = currentUserId,
                SenderRole   = SenderRole.STUDENT,
                Content      = req.Content,
                CreatedAt    = now
            });
            await _uow.questionRepository.UpdateAsync(question);
            await _uow.SaveChangeAsync();

            // 5. Reload với messages để trả về
            var detail = await _uow.questionRepository.GetWithMessagesAsync(question.Id);
            return ApiResponse<QuestionDetailDto>.Ok(MapToDetail(detail!));
        }

        // ════════════════════════════════════════════════════════════════
        // STUDENT — Danh sách câu hỏi của nhóm
        // ════════════════════════════════════════════════════════════════
        public async Task<ApiResponse<IEnumerable<QuestionListItemDto>>> GetStudentQuestionsAsync(
            int currentUserId, int groupId, string? status)
        {
            var isMember = await _uow.groupMemberRepository.IsMemberAsync(groupId, currentUserId);
            if (!isMember)
                return ApiResponse<IEnumerable<QuestionListItemDto>>.Fail(
                    ReasonCodes.NOT_GROUP_MEMBER, "Bạn không thuộc nhóm này.");

            var questions = await _uow.questionRepository.GetByGroupAsync(groupId, status);
            return ApiResponse<IEnumerable<QuestionListItemDto>>.Ok(
                questions.Select(MapToListItem));
        }

        // ════════════════════════════════════════════════════════════════
        // STUDENT — Chi tiết câu hỏi + thread messages
        // ════════════════════════════════════════════════════════════════
        public async Task<ApiResponse<QuestionDetailDto>> GetQuestionDetailForStudentAsync(
            int currentUserId, int questionId)
        {
            var question = await _uow.questionRepository.GetWithMessagesAsync(questionId);
            if (question == null)
                return ApiResponse<QuestionDetailDto>.Fail(
                    ReasonCodes.QUESTION_NOT_FOUND, "Không tìm thấy câu hỏi.");

            var isMember = await _uow.groupMemberRepository.IsMemberAsync(question.GroupID, currentUserId);
            if (!isMember)
                return ApiResponse<QuestionDetailDto>.Fail(
                    ReasonCodes.NOT_GROUP_MEMBER, "Bạn không có quyền xem câu hỏi này.");

            return ApiResponse<QuestionDetailDto>.Ok(MapToDetail(question));
        }

        // ════════════════════════════════════════════════════════════════
        // STUDENT — Gửi tin nhắn (re-open nếu ANSWERED/CLOSED)
        // ════════════════════════════════════════════════════════════════
        public async Task<ApiResponse<QuestionDetailDto>> AddStudentMessageAsync(
            int currentUserId, int questionId, AddMessageRequest req)
        {
            var question = await _uow.questionRepository.GetWithMessagesAsync(questionId);
            if (question == null)
                return ApiResponse<QuestionDetailDto>.Fail(
                    ReasonCodes.QUESTION_NOT_FOUND, "Không tìm thấy câu hỏi.");

            var isMember = await _uow.groupMemberRepository.IsMemberAsync(question.GroupID, currentUserId);
            if (!isMember)
                return ApiResponse<QuestionDetailDto>.Fail(
                    ReasonCodes.NOT_GROUP_MEMBER, "Bạn không thuộc nhóm này.");

            // Re-open rule: student reply vào ANSWERED/CLOSED → PENDING
            if (question.Status is "ANSWERED" or "CLOSED")
                question.Status = "PENDING";

            var now = DateTime.UtcNow;
            question.Messages.Add(new QuestionMessage
            {
                QuestionId   = questionId,
                SenderUserId = currentUserId,
                SenderRole   = SenderRole.STUDENT,
                Content      = req.Content,
                CreatedAt    = now
            });
            question.LastReplyAt        = now;
            question.LastRepliedByUserId = currentUserId;
            question.UpdatedAt          = now;

            await _uow.questionRepository.UpdateAsync(question);
            await _uow.SaveChangeAsync();

            var updated = await _uow.questionRepository.GetWithMessagesAsync(questionId);
            return ApiResponse<QuestionDetailDto>.Ok(MapToDetail(updated!));
        }

        // ════════════════════════════════════════════════════════════════
        // TEACHER — Danh sách câu hỏi theo topic
        // ════════════════════════════════════════════════════════════════
        public async Task<ApiResponse<IEnumerable<QuestionListItemDto>>> GetTeacherQuestionsAsync(
            int currentUserId, int topicId, string? status)
        {
            // Topic.TeacherID = Teacher.TeacherID = User.UserID
            var topic = await _uow.topicRepository.GetByIdAsync(topicId);
            if (topic == null || topic.TeacherID != currentUserId)
                return ApiResponse<IEnumerable<QuestionListItemDto>>.Fail(
                    ReasonCodes.NOT_TOPIC_OWNER, "Bạn không phụ trách topic này.");

            var questions = await _uow.questionRepository.GetByTopicAsync(topicId, status);
            return ApiResponse<IEnumerable<QuestionListItemDto>>.Ok(
                questions.Select(MapToListItem));
        }

        // ════════════════════════════════════════════════════════════════
        // TEACHER — Cập nhật status (forward-only)
        // ════════════════════════════════════════════════════════════════
        public async Task<ApiResponse<bool>> UpdateQuestionStatusAsync(
            int currentUserId, int questionId, UpdateStatusRequest req)
        {
            var question = await _uow.questionRepository.GetWithMessagesAsync(questionId);
            if (question == null)
                return ApiResponse<bool>.Fail(
                    ReasonCodes.QUESTION_NOT_FOUND, "Không tìm thấy câu hỏi.");

            var topic = await _uow.topicRepository.GetByIdAsync(question.TopicID);
            if (topic == null || topic.TeacherID != currentUserId)
                return ApiResponse<bool>.Fail(
                    ReasonCodes.NOT_TOPIC_OWNER, "Bạn không phụ trách topic này.");

            var target = req.Status.Trim().ToUpper();
            if (!StatusOrder.Contains(target))
                return ApiResponse<bool>.Fail(
                    ReasonCodes.INVALID_STATUS, $"Status '{req.Status}' không hợp lệ.");

            int currentIdx = Array.IndexOf(StatusOrder, question.Status);
            int targetIdx  = Array.IndexOf(StatusOrder, target);
            if (targetIdx != currentIdx + 1)
                return ApiResponse<bool>.Fail(
                    ReasonCodes.INVALID_STATUS_TRANSITION,
                    $"Chỉ được chuyển tuần tự. {question.Status} → {StatusOrder[currentIdx + 1]}");

            question.Status    = target;
            question.UpdatedAt = DateTime.UtcNow;

            await _uow.questionRepository.UpdateAsync(question);
            await _uow.SaveChangeAsync();

            return ApiResponse<bool>.Ok(true, $"Đã chuyển trạng thái → {target}.");
        }

        // ════════════════════════════════════════════════════════════════
        // TEACHER — Trả lời (auto ANSWERED)
        // ════════════════════════════════════════════════════════════════
        public async Task<ApiResponse<QuestionDetailDto>> AddTeacherMessageAsync(
            int currentUserId, int questionId, AddMessageRequest req)
        {
            var question = await _uow.questionRepository.GetWithMessagesAsync(questionId);
            if (question == null)
                return ApiResponse<QuestionDetailDto>.Fail(
                    ReasonCodes.QUESTION_NOT_FOUND, "Không tìm thấy câu hỏi.");

            var topic = await _uow.topicRepository.GetByIdAsync(question.TopicID);
            if (topic == null || topic.TeacherID != currentUserId)
                return ApiResponse<QuestionDetailDto>.Fail(
                    ReasonCodes.NOT_TOPIC_OWNER, "Bạn không phụ trách topic này.");

            var now = DateTime.UtcNow;
            question.Messages.Add(new QuestionMessage
            {
                QuestionId   = questionId,
                SenderUserId = currentUserId,
                SenderRole   = SenderRole.TEACHER,
                Content      = req.Content,
                CreatedAt    = now
            });
            // Teacher reply → auto ANSWERED
            question.Status             = "ANSWERED";
            question.LastReplyAt        = now;
            question.LastRepliedByUserId = currentUserId;
            question.UpdatedAt          = now;

            await _uow.questionRepository.UpdateAsync(question);
            await _uow.SaveChangeAsync();

            var updated = await _uow.questionRepository.GetWithMessagesAsync(questionId);
            return ApiResponse<QuestionDetailDto>.Ok(MapToDetail(updated!));
        }

        // ════════════════════════════════════════════════════════════════
        // Private mapping helpers
        // ════════════════════════════════════════════════════════════════
        private static QuestionListItemDto MapToListItem(Question q) => new(
            Id:          q.Id,
            Title:       q.Title,
            Content:     q.Content.Length > 150 ? q.Content[..150] + "…" : q.Content,
            Status:      q.Status,
            StudentName: q.Student?.User?.FullName ?? "Unknown",
            TopicName:   q.Topic?.TopicName ?? "—",
            CreatedAt:   q.CreatedAt,
            LastReplyAt: q.LastReplyAt,
            MessageCount: q.Messages?.Count ?? 0
        );

        private static QuestionDetailDto MapToDetail(Question q) => new(
            Id:          q.Id,
            Title:       q.Title,
            Content:     q.Content,
            Status:      q.Status,
            StudentName: q.Student?.User?.FullName ?? "Unknown",
            TopicName:   q.Topic?.TopicName ?? "—",
            GroupName:   q.StudentGroup?.GroupName ?? "—",
            CreatedAt:   q.CreatedAt,
            Messages:    q.Messages?
                          .OrderBy(m => m.CreatedAt)
                          .Select(m => new MessageDto(
                              m.Id,
                              m.Sender?.FullName ?? "Unknown",
                              m.SenderRole.ToString(),
                              m.Content,
                              m.CreatedAt))
                         ?? []
        );
    }
}
