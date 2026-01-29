using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IUserRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangeAsync();
        IUserRepository userRepository { get; set; }
        IRoleRepository roleRepository { get; set; }
    }
}
