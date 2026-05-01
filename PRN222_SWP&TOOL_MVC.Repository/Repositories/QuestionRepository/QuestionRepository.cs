using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IQuestionRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.QuestionRepository
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
