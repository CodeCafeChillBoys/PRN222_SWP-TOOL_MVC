using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IUserRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork.IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IUserRepository userRepository { get; set; }
        public IRoleRepository roleRepository { get; set; }

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository , IRoleRepository roleRepository)
        {
            this._dbContext = dbContext;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
