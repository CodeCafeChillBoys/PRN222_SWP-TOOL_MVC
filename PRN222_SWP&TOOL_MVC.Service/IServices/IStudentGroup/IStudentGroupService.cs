using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup
{
    public interface IStudentGroupService
    {
        Task<StudentGroup> CreateGroupAsync(int classId, int studentId, string groupName);
        Task<bool> JoinGroupAsync(string inviteCode, int studentId);
        Task<StudentGroup> GetGroupInfoAsync(int groupId);
        Task<List<Class>> GetClassesBySemesterAsync(int semesterId);
        Task<GroupInfoResponseDTO?> GetMyGroupAsync(int studentId);
    }
}
