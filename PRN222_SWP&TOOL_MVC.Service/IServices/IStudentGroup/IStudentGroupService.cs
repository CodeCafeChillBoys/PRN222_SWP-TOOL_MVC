using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup
{
    public interface IStudentGroupService 
    {

        Task<ReturnData<int>> CreateGroupAsync(CreateGroupRequestDTO request, int creatorUserId );
        Task<ReturnData<bool>> AddMemberAsync(int groupId, int studentId);
        Task<ReturnData<bool>> RemoveMemberAsync(int groupId, int studentId);
        Task<ReturnData<GroupInfoResponseDTO>> GetGroupInfoAsync(int groupId);
    }
}
