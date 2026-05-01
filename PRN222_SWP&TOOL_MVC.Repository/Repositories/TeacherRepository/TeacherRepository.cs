using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITeacherRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.TeacherRepository
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(AppDbContext context) : base(context)
        {
        }
    }
}
