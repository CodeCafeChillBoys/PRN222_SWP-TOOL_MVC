using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ClassID          = request.ClassID,
                GroupName        = request.GroupName.Trim(),
                CreatedByUserID  = creatorUserId,
                IsLocked         = false,
                MaxMember        = request.MaxMember > 0 ? request.MaxMember : 5,
                Status           = "ACTIVE",
                // Sinh mã mời ngẫu nhiên 8 ký tự — viết hoa để dễ nhập
                InviteCode       = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                CreatedAt        = DateTime.UtcNow
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

        public async Task<ReturnData<int>> JoinGroupAsync(string inviteCode, int studentId)
        {
            if (string.IsNullOrWhiteSpace(inviteCode))
                return new ReturnData<int> { Success = false, ResponseMessage = "Vui lòng nhập mã mời." };

            var group = await _uow.studentGroupRepository.GetByInviteCodeAsync(inviteCode);
            if (group == null)
                return new ReturnData<int> { Success = false, ResponseMessage = $"Mã mời '{inviteCode.ToUpper()}' không hợp lệ." };

            if (group.IsLocked)
                return new ReturnData<int> { Success = false, ResponseMessage = "Nhóm này đã bị khoá, không thể tham gia." };

            var memberCount = await _uow.groupMemberRepository.CountByGroupAsync(group.GroupID);
            if (memberCount >= group.MaxMember)
                return new ReturnData<int> { Success = false, ResponseMessage = $"Nhóm đã đầy ({memberCount}/{group.MaxMember} thành viên)." };

            var alreadyMember = await _uow.groupMemberRepository.IsMemberAsync(group.GroupID, studentId);
            if (alreadyMember)
                return new ReturnData<int> { Success = false, ResponseMessage = "Bạn đã là thành viên của nhóm này." };

            var member = new GroupMember
            {
                GroupID   = group.GroupID,
                StudentID = studentId,
                IsLeader  = false
            };
            await _uow.groupMemberRepository.AddAsync(member);
            await _uow.SaveChangeAsync();

            // Tự động khoá khi đủ MaxMember
            var newCount = memberCount + 1;
            if (newCount >= group.MaxMember)
            {
                group.IsLocked = true;
                await _uow.studentGroupRepository.UpdateAsync(group);
                await _uow.SaveChangeAsync();
            }

            return new ReturnData<int> { Success = true, ResponseMessage = "Tham gia nhóm thành công!", Data = group.GroupID };
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
