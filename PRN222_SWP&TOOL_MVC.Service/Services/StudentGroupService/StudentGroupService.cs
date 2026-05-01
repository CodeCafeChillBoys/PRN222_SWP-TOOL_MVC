using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup;

namespace PRN222_SWP_TOOL_MVC.Service.Services.StudentGroupService
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StudentGroup> CreateGroupAsync(int classId, int studentId, string groupName)
        {
            // Tạo mã kí tự lấy từ 0 đến 0 đc định dạng bằng hex
            string inviteCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            var newGroup = new StudentGroup
            {
                GroupName = groupName,
                ClassID = classId,
                CreatedByUserID = studentId,
                MaxMember = 6,
                InviteCode = inviteCode,
                IsLocked = false,
                Status = "Active"
            };
            // Tạo ra một group mới đc leader tạo
            await _unitOfWork.studentGroupRepository.AddAsync(newGroup);
            await _unitOfWork.SaveChangeAsync();

            var member = new GroupMember
            {
                GroupID = newGroup.GroupID,
                StudentID = studentId,
                IsLeader = true
            };

            await _unitOfWork.groupMemberRepository.AddAsync(member);
            await _unitOfWork.SaveChangeAsync();

            return newGroup;
        }


        public async Task<List<Class>> GetClassesBySemesterAsync(int semesterId)
        {
            // Lấy ra danh sách class
            var classes = await _unitOfWork.classRepository.GetAllAsync();
            // check semester == 0 thì trả về class
            if (semesterId == 0) return classes.ToList();
            // lấy lên classs có semeter
            return classes.Where(c => c.SemesterID == semesterId).ToList();
        }

        public async Task<StudentGroup> GetGroupInfoAsync(int groupId)
        {
            // lấy lên danh sách group theo ID
            return await _unitOfWork.studentGroupRepository.GetByIdAsync(groupId);
        }

        /// <summary>
        /// Lấy nhóm mà student đang thuộc về, kèm danh sách thành viên
        /// </summary>
        public async Task<GroupInfoResponseDTO?> GetMyGroupAsync(int studentId)
        {
            // 1. Tìm GroupMember record của student này
            var allMembers = await _unitOfWork.groupMemberRepository.GetAllAsync();
            // lấy lên studentID là tk leader
            var myMembership = allMembers.FirstOrDefault(m => m.StudentID == studentId);
            if (myMembership == null) return null;

            // 2. Lấy Group của tk leader đã chọn
            var group = await _unitOfWork.studentGroupRepository.GetByIdAsync(myMembership.GroupID);
            if (group == null) return null;

            // 3. Lấy tất cả thành viên của nhóm đó
            // Sau khi lấy lên groupID và xem từng thanh viên
            var membersInGroup = allMembers.Where(m => m.GroupID == group.GroupID).ToList();

            // 4. Map sang DTO — dùng GetWithUserAsync để load kèm User.FullName
            // lấy ra từng thanh viên
            var memberDTOs = new List<GroupMemberResponseDTO>();
            foreach (var m in membersInGroup)
            {
                var student = await _unitOfWork.studentRepository.GetWithUserAsync(m.StudentID);
                memberDTOs.Add(new GroupMemberResponseDTO
                {
                    StudentID = m.StudentID,
                    FullName = student?.User?.FullName ?? $"Student #{m.StudentID}",
                    IsLeader = m.IsLeader
                });
            }

            return new GroupInfoResponseDTO
            {
                GroupID = group.GroupID,
                GroupName = group.GroupName,
                IsLocked = group.IsLocked,
                Status = group.Status,
                ClassID = group.ClassID,
                MaxMember = group.MaxMember,
                InviteCode = group.InviteCode,
                MemberCount = membersInGroup.Count,
                Members = memberDTOs
            };
        }

        public async Task<bool> JoinGroupAsync(string inviteCode, int studentId)
        {
            // lấy lên danh sách group
            var groups = await _unitOfWork.studentGroupRepository.GetAllAsync();
            // Check inviteCode
            var targetGroup = groups.FirstOrDefault(g => g.InviteCode == inviteCode);
            // if == null or locked = true thì return false
            if (targetGroup == null || targetGroup.IsLocked)
                return false;
            // lấy lên groupMembers
            var groupMembers = await _unitOfWork.groupMemberRepository.GetAllAsync();
            // Check groupID exist
            var membersInGroup = groupMembers.Where(m => m.GroupID == targetGroup.GroupID).ToList();

            // nếu nhóm đã đủ thành viên thì return false
            if (membersInGroup.Count >= targetGroup.MaxMember)
            {
                targetGroup.IsLocked = true;
                await _unitOfWork.studentGroupRepository.UpdateAsync(targetGroup);
                await _unitOfWork.SaveChangeAsync();
                return false;
            }

            // Không cho join nếu đã là thành viên
            if (membersInGroup.Any(m => m.StudentID == studentId))
                return false;

            var newMember = new GroupMember
            {
                GroupID = targetGroup.GroupID,
                StudentID = studentId,
                IsLeader = false
            };

            await _unitOfWork.groupMemberRepository.AddAsync(newMember);

            // sau khi thêm vào thì check cái này full chưa nếu full rồi đóng luôn
            if (membersInGroup.Count + 1 >= targetGroup.MaxMember)
            {
                targetGroup.IsLocked = true;
                await _unitOfWork.studentGroupRepository.UpdateAsync(targetGroup);
            }
            await _unitOfWork.SaveChangeAsync();
            return true;
        }
    }
}
