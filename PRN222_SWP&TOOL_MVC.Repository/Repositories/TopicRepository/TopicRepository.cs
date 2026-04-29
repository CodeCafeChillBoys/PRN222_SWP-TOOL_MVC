using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITopRepositroy;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.TopicRepository
{
    public class TopicRepository : GenericRepository<Topic>, ITopicRepository
    {
        public TopicRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Topic>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Topic>()
                .Include(t => t.Teacher)
                    .ThenInclude(te => te.User)
                .Include(t => t.TopicRegistrations)
                .ToListAsync();
        }
    }
}
