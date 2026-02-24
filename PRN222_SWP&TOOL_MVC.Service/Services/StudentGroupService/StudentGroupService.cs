using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup;

namespace PRN222_SWP_TOOL_MVC.Service.Services.StudentGroupService
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IUnitOfWork _uow;
        private readonly AppDbContext _db;

        public StudentGroupService(IUnitOfWork uow, AppDbContext db)
        {
            _uow = uow;
            _db = db;
        }

        public Task<ReturnData<bool>> AddMemberAsync(int groupId, int studentId)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnData<int>> CreateGroupAsync(CreateGroupRequestDTO request, int creatorUserId)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.GroupName))
            {
                return new ReturnData<int>
                {
                    Success = false,
                    ResponseMessage = "GroupName is required"
                };
            }

            using var tx = await _db.Database.BeginTransactionAsync();

            var group = new StudentGroup
            {
                ClassID = request.ClassID,
                GroupName = request.GroupName.Trim(),
                CreatedByUserID = creatorUserId,
                IsLocked = false
            };

            await _uow.studentGroupRepository.AddAsync(group);
            await _uow.SaveChangeAsync();

            var leader = new GroupMember
            {
                GroupID = group.GroupID,
                StudentID = creatorUserId,
                IsLeader = true
            };

            await _uow.groupMemberRepository.AddAsync(leader);
            await _uow.SaveChangeAsync();

            await tx.CommitAsync();

            return new ReturnData<int>
            {
                Success = true,
                ResponseMessage = "Create group successfully",
                Data = group.GroupID
            };
        }

        public async Task<ReturnData<GroupInfoResponseDTO>> GetGroupInfoAsync(int groupId)
        {
            var group = await _uow.studentGroupRepository.GetGroupWithMembersAsync(groupId);
            if (group == null)
            {
                return new ReturnData<GroupInfoResponseDTO>
                {
                    Success = false,
                    ResponseMessage = "Group not found",
                    Data = null
                };
            }

            var dto = new GroupInfoResponseDTO
            {
                GroupID = group.GroupID,
                GroupName = group.GroupName,
                IsLocked = group.IsLocked,
                Status = group.Status,

                ClassID = group.ClassID,
                MaxMember = group.MaxMember,
                InviteCode = group.InviteCode,

                MemberCount = group.Members?.Count ?? 0,
                Members = (group.Members ?? new List<GroupMember>())
                    .Select(m => new GroupMemberResponseDTO
                    {
                        StudentID = m.StudentID,
                        IsLeader = m.IsLeader,

                        FullName = m.Student?.User?.FullName ?? ""

                    })
                    .ToList()
            };

            return new ReturnData<GroupInfoResponseDTO>
            {
                Success = true,
                ResponseMessage = "OK",
                Data = dto
            };
        }

        public Task<ReturnData<bool>> RemoveMemberAsync(int groupId, int studentId)
        {
            throw new NotImplementedException();
        }

        Task<ReturnData<GroupInfoResponseDTO>> IStudentGroupService.GetGroupInfoAsync(int groupId)
        {
            throw new NotImplementedException();
        }
    }
}
