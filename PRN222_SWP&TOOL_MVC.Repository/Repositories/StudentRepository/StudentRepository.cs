using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IStudentRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.StudentRepository
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Student?> GetWithUserAsync(int studentId)
        {
            return await _context.Set<Student>()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);
        }
    }
}
