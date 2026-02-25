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
        private readonly IUnitOfWork  _unitOfWork;
        private readonly AppDbContext _db;

        public StudentService(IUnitOfWork unitOfWork, AppDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db         = db;
        }

        public async Task<StudentDashboardRequestDTO> GetDashboardAsync(string tab)
        {
            // 1. Lấy nhóm trước — kèm Members để đỡ NullReference
            var groupsRaw = await _unitOfWork.studentGroupRepository
                .GetAllWithIncludeAsync(g => g.Members);
            var group = groupsRaw.FirstOrDefault();

            // 2. Lấy topic (tất cả — student xem để chọn đề tài)
            var topics = (await _unitOfWork.topicRepository.GetAllAsync()).ToList();

            // 3. Lấy questions kèm Messages, lọc theo nhóm của student
            var allQuestions = (await _unitOfWork.questionRepository
                .GetAllWithIncludeAsync(q => q.Messages)).ToList();

            var questions = group != null
                ? allQuestions.Where(q => q.GroupID == group.GroupID).ToList()
                : new List<Question>();


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

            // 4. Lấy TopicRegistration của nhóm
            TopicRegistration? reg = null;
            if (group != null)
            {
                reg = await _db.TopicRegistrations
                    .Include(tr => tr.Topic)
                    .FirstOrDefaultAsync(tr => tr.GroupID == group.GroupID);
            }

            return new StudentDashboardRequestDTO
            {
                CurrentTab         = tab ?? "group",
                Group              = groupViewModel,
                Topics             = topics,
                Questions          = questions,
                RegisteredTopicId  = reg?.TopicID,
                RegisteredTopicName = reg?.Topic?.TopicName
            };
        }
    }
}
