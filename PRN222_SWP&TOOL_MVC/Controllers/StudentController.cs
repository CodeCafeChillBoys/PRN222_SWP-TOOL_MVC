using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Models.StudentViewModels;
using PRN222_SWP_TOOL_MVC.Models.GroupViewModels;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup;
using System.Security.Claims;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    private readonly IStudentGroupService _groupService;

    public StudentController(IStudentGroupService groupService)
    {
        _groupService = groupService;
    }

    private int GetCurrentUserId()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
        {
            throw new InvalidOperationException("User is not authenticated");
        }
        return int.Parse(userIdStr);
    }

    public async Task<IActionResult> Index(int groupId = 0)
    {
        var vm = new StudentIndexViewModel();

        // Tạm thời: nếu bạn truyền groupId vào Index thì load group
        // Chuẩn hơn là theo classId -> MyGroup (mình làm sau)
        if (groupId > 0)
        {
            var result = await _groupService.GetGroupInfoAsync(groupId);
            if (result.Success && result.Data != null)
            {
                vm.Group = new GroupDetailsViewModel
                {
                    GroupID = result.Data.GroupID,
                    GroupName = result.Data.GroupName,
                    IsLocked = result.Data.IsLocked,
                    Status = result.Data.Status,
                    ClassID = result.Data.ClassID,
                    MaxMember = result.Data.MaxMember,
                    InviteCode = result.Data.InviteCode,
                    MemberCount = result.Data.MemberCount,
                    Members = result.Data.Members.Select(m => new GroupMemberItemViewModel
                    {
                        StudentID = m.StudentID,
                        FullName = m.FullName,
                        IsLeader = m.IsLeader
                    }).ToList()
                };
            }
            else
            {
                ViewBag.Message = result.ResponseMessage ?? "Bạn chưa có nhóm.";
            }
        }
        else
        {
            ViewBag.Message = "Bạn chưa có nhóm.";
        }

        return View(vm);
    }
}