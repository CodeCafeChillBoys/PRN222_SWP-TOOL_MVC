using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITopicRegistrationsRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.TopicRegistrationsRepository
{
    public class TopicRegistrationsRepository : GenericRepository<TopicRegistration>, ITopicRegistrationsRepository
    {
        public TopicRegistrationsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
