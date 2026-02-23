using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IQuestionRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.QuestionRepository
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _ctx;

        public QuestionRepository(AppDbContext context) : base(context)
        {
            _ctx = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Question>> GetByGroupAsync(
            int groupId, string? status, CancellationToken ct = default)
        {
            var query = _ctx.Questions
                .Include(q => q.Topic)
                .Include(q => q.Student).ThenInclude(s => s.User)
                .Where(q => q.GroupID == groupId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(q => q.Status == status.ToUpper());

            return await query.OrderByDescending(q => q.LastReplyAt).ToListAsync(ct);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Question>> GetByTopicAsync(
            int topicId, string? status, CancellationToken ct = default)
        {
            var query = _ctx.Questions
                .Include(q => q.StudentGroup)
                .Include(q => q.Student).ThenInclude(s => s.User)
                .Where(q => q.TopicID == topicId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(q => q.Status == status.ToUpper());

            return await query.OrderByDescending(q => q.LastReplyAt).ToListAsync(ct);
        }

        /// <inheritdoc />
        public async Task<Question?> GetWithMessagesAsync(
            int questionId, CancellationToken ct = default)
        {
            return await _ctx.Questions
                .Include(q => q.Topic)
                .Include(q => q.StudentGroup)
                .Include(q => q.Student).ThenInclude(s => s.User)
                .Include(q => q.Messages.OrderBy(m => m.CreatedAt))
                    .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(q => q.Id == questionId, ct);
        }
    }
}

