using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.IRepositories.IStudentRepository
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetWithUserAsync(int studentId);
    }
}
