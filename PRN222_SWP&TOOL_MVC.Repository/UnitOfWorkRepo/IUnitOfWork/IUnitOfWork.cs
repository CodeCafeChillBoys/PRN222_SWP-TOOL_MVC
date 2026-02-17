using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IClassRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupMemberRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ISemesterRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IUserRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangeAsync();
        IUserRepository userRepository { get; set; }
        IRoleRepository roleRepository { get; set; }
        ISemesterRepository semesterRepository { get; set; }
        IClassRepository classRepository { get; set; }
        IStudentGroupRepository studentGroupRepository { get; set; }
        IGroupMemberRepository groupMemberRepository { get; set; }
    }
}
