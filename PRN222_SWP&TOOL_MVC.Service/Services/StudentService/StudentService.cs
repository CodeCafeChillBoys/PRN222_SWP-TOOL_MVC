using Microsoft.EntityFrameworkCore;
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
            // Lấy danh sách topic (không cần include phức tạp)
            var topics = (await _unitOfWork.topicRepository.GetAllAsync()).ToList();

            // Lấy danh sách questions (chỉ lấy basic, không include navigation)
            var questions = (await _unitOfWork.questionRepository.GetAllAsync()).ToList();

            // Lấy nhóm đầu tiên — kèm Members để tránh NullReference
            // Dùng GetAllWithIncludeAsync để eager-load Members
            var groupsRaw = await _unitOfWork.studentGroupRepository
                .GetAllWithIncludeAsync(g => g.Members);
            var group = groupsRaw.FirstOrDefault();

            GroupDetailsViewModel groupViewModel;
            if (group != null)
            {
                groupViewModel = new GroupDetailsViewModel
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
                            // Student navigation không được load ở đây — dùng StudentID làm fallback
                            FullName = m.Student?.User?.FullName ?? $"Student #{m.StudentID}",
                            IsLeader = m.IsLeader
                        }).ToList()
                        : new List<GroupMemberItemViewModel>()
                };
            }
            else
            {
                groupViewModel = new GroupDetailsViewModel
                {
                    GroupID = 0,
                    GroupName = "Chưa có nhóm",
                    Members = new List<GroupMemberItemViewModel>()
                };
            }

            return new StudentDashboardRequestDTO
            {
                CurrentTab = tab ?? "group",
                Group = groupViewModel,
                Topics = topics,
                Questions = questions
            };
        }
    }
}
