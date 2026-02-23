using System.Collections.Generic;

namespace PRN222_SWP_TOOL_MVC.Models.GroupViewModels
{
    public class GroupDetailsViewModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }

        public bool IsLocked { get; set; }
        public string Status { get; set; }

        public int ClassID { get; set; }
        public int MaxMember { get; set; }
        public string InviteCode { get; set; }

        public int MemberCount { get; set; }
        public List<GroupMemberItemViewModel> Members { get; set; } = new();
    }

    public class GroupMemberItemViewModel
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public bool IsLeader { get; set; }
    }
}