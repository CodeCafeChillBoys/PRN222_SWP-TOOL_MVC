using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Service.DTO.Response
{
    public class GroupInfoResponseDTO
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public bool IsLocked { get; set; }
        public string Status { get; set; }

        public int ClassID { get; set; }
        public int MaxMember { get; set; }
        public string InviteCode { get; set; }

        public int MemberCount { get; set; }
        public List<GroupMemberResponseDTO> Members { get; set; } = new();
    }

    public class GroupMemberResponseDTO
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public bool IsLeader { get; set; }
    }
}
