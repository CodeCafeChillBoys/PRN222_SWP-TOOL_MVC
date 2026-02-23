using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.IRepositories.IQuestionRepository
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        /// <summary>Lấy danh sách câu hỏi của một nhóm (cho student)</summary>
        Task<IEnumerable<Question>> GetByGroupAsync(int groupId, string? status, CancellationToken ct = default);

        /// <summary>Lấy danh sách câu hỏi thuộc một topic (cho teacher)</summary>
        Task<IEnumerable<Question>> GetByTopicAsync(int topicId, string? status, CancellationToken ct = default);

        /// <summary>Lấy câu hỏi kèm toàn bộ thread messages (eager load)</summary>
        Task<Question?> GetWithMessagesAsync(int questionId, CancellationToken ct = default);
    }
}

