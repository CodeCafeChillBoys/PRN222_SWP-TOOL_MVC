using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudent;
using PRN222_SWP_TOOL_MVC.Service.Models;

namespace PRN222_SWP_TOOL_MVC.Service.Services.StudentService
{
    public class StudentService : IStudentService
    {

        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StudentDashboardRequestDTO> GetDashboardAsync(string tab)
        {
            var groups = await _unitOfWork.studentGroupRepository.GetAllAsync();
            var topics = await _unitOfWork.topicRepository.GetAllAsync();
            var questions = await _unitOfWork.questionRepository.GetAllAsync();

            // TODO: Sau này filter theo StudentID
            var group = groups.FirstOrDefault();

            var groupViewModel = group != null
                ? new GroupDetailsViewModel
                {
                    GroupID = group.GroupID,
                    GroupName = group.GroupName,
                    IsLocked = group.IsLocked,
                    Status = group.Status,
                    ClassID = group.ClassID,
                    MaxMember = group.MaxMember,
                    InviteCode = group.InviteCode,

                    MemberCount = group.Members?.Count ?? 0,

                    Members = group.Members != null
                        ? group.Members.Select(m => new GroupMemberItemViewModel
                        {
                            StudentID = m.StudentID,
                            FullName = m.Student?.User.FullName ?? "",
                            IsLeader = m.IsLeader
                        }).ToList()
                        : new List<GroupMemberItemViewModel>()
                }
                : new GroupDetailsViewModel(); // GroupID = 0

            return new StudentDashboardRequestDTO
            {
                CurrentTab = tab,
                Group = groupViewModel,
                Topics = topics?.ToList() ?? new List<Topic>(),
                Questions = questions?.ToList() ?? new List<Question>()
            };
        }
    }
}
