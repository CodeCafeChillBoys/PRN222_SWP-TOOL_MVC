using PRN222_SWP_TOOL_MVC.Models.GroupViewModels;

namespace PRN222_SWP_TOOL_MVC.Models.StudentViewModels
{
    public class StudentIndexViewModel
    {
        public GroupDetailsViewModel Group { get; set; } = new GroupDetailsViewModel
        {
            GroupID = 0,
            GroupName = "(Chưa có nhóm)",
            IsLocked = false,
            Status = "EMPTY",
            MaxMember = 6,
            MemberCount = 0
        };

        // sau này bạn thêm Topic, QA vào đây
    }
}