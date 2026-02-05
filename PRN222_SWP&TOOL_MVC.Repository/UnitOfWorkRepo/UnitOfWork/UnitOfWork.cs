using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IClassRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupMemberRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ISemesterRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IUserRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork.IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IUserRepository userRepository { get; set; }
        public IRoleRepository roleRepository { get; set; }
        public ISemesterRepository semesterRepository { get; set; }

        public IClassRepository classRepository { get; set; }   
        public IStudentGroupRepository studentGroupRepository { get; set; }
        public IGroupMemberRepository groupMemberRepository { get; set; }
        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository , IRoleRepository roleRepository, ISemesterRepository semesterRepository, 
                            IClassRepository classRepository, IStudentGroupRepository studentGroupRepository, IGroupMemberRepository groupMemberRepository)
        {
            this._dbContext = dbContext;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.semesterRepository = semesterRepository;
            this.classRepository = classRepository;
            this.studentGroupRepository = studentGroupRepository;
            this.groupMemberRepository = groupMemberRepository;
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
