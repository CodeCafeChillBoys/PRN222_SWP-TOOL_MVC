using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IClassRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IQuestionRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ISemesterRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IStudentRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITeacherRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITopicRegistrationsRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITopRepositroy;
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
        public IStudentRepository studentRepository { get; set; }
        public ITeacherRepository teacherRepository { get; set; }
        public IQuestionRepository questionRepository { get; set; }
        public ITopicRepository topicRepository { get; set; }
        public ITopicRegistrationsRepository topicRegistrationsRepository { get; set; }
        public PRN222_SWP_TOOL_MVC.Repository.IRepositories.IStudentGroupRepository.IStudentGroupRepository studentGroupRepository { get; set; }
        public PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupMemberRepository.IGroupMemberRepository groupMemberRepository { get; set; }

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IRoleRepository roleRepository, ISemesterRepository semesterRepository,
                            IStudentRepository studentRepository, ITeacherRepository teacherRepository, IQuestionRepository questionRepository, ITopicRepository topicRepository,
                          ITopicRegistrationsRepository topicRegistrationsRepository, IClassRepository classRepository,
                          PRN222_SWP_TOOL_MVC.Repository.IRepositories.IStudentGroupRepository.IStudentGroupRepository studentGroupRepository,
                          PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupMemberRepository.IGroupMemberRepository groupMemberRepository)
        {
            this._dbContext = dbContext;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.semesterRepository = semesterRepository;
            this.classRepository = classRepository;
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.questionRepository = questionRepository;
            this.topicRepository = topicRepository;
            this.topicRegistrationsRepository = topicRegistrationsRepository;
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
