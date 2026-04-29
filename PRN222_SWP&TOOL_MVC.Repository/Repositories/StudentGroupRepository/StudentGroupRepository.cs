using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IStudentGroupRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.StudentGroupRepository
{
    public class StudentGroupRepository : GenericRepository<StudentGroup>, IStudentGroupRepository
    {
        public StudentGroupRepository(AppDbContext context) : base(context)
        {
        }
    }
}
