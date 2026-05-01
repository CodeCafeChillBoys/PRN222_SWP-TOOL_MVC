using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITopic;
using System.Security.Claims;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    private readonly IStudentGroupService _studentGroupService;
    private readonly ITopicService _topicService;

    public StudentController(IStudentGroupService studentGroupService, ITopicService topicService)
    {
        _studentGroupService = studentGroupService;
        _topicService = topicService;
    }

    public async Task<IActionResult> Index(string tab = "group")
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // ── Nhóm hiện tại ──────────────────────────────────────
        var myGroup = await _studentGroupService.GetMyGroupAsync(userId);
        ViewBag.MyGroup = myGroup;

        // ── Danh sách lớp học cho dropdown ──────────────────────
        ViewBag.Classes = await _studentGroupService.GetClassesBySemesterAsync(0);

        // ── Topics + trạng thái đã đăng ký ──────────────────────
        var topics = await _topicService.GetTopicsWithDetailsAsync();
        int? selectedId = myGroup != null  // nhóm tồn tại
            ? await _topicService.GetRegisteredTopicIdAsync(myGroup.GroupID)
            : null;

        var model = new StudentDashboardRequestDTO
        {
            CurrentTab = tab,
            Topics = topics,
            Questions = new List<Question>(),
            SelectedTopicId = selectedId
        };
        return View(model);
    }

    // ── Tạo nhóm ──────────────────────────────────────────────
    [HttpPost]
    public async Task<IActionResult> CreateGroup(int classId, string groupName)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var group = await _studentGroupService.CreateGroupAsync(classId, userId, groupName);
        TempData["SuccessMessage"] = $"Tạo nhóm \"{group.GroupName}\" thành công! Mã mời: {group.InviteCode}";
        return RedirectToAction("Index", new { tab = "group" });
    }

    // ── Tham gia nhóm ─────────────────────────────────────────
    [HttpPost]
    public async Task<IActionResult> JoinGroup(string inviteCode)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var success = await _studentGroupService.JoinGroupAsync(inviteCode, userId);

        if (success)
            TempData["SuccessMessage"] = "Tham gia nhóm thành công!";
        else
            TempData["ErrorMessage"] = "Tham gia thất bại! Nhóm đã đủ người, mã sai, hoặc bạn đã ở trong nhóm khác.";

        return RedirectToAction("Index", new { tab = "group" });
    }

    // ── Đăng ký đề tài ────────────────────────────────────────
    [HttpPost]
    public async Task<IActionResult> RegisterTopic(int topicId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var myGroup = await _studentGroupService.GetMyGroupAsync(userId);

        if (myGroup == null)
        {
            TempData["ErrorMessage"] = "Bạn chưa có nhóm. Hãy tạo hoặc tham gia nhóm trước!";
            return RedirectToAction("Index", new { tab = "topic" });
        }

        // Chỉ trưởng nhóm được đăng ký
        var leader = myGroup.Members.FirstOrDefault(m => m.IsLeader);
        if (leader == null || leader.StudentID != userId)
        {
            TempData["ErrorMessage"] = "Chỉ trưởng nhóm mới có thể chọn đề tài!";
            return RedirectToAction("Index", new { tab = "topic" });
        }

        var (success, message) = await _topicService.RegisterTopicAsync(topicId, myGroup.GroupID);

        if (success)
            TempData["SuccessMessage"] = message;
        else
            TempData["ErrorMessage"] = message;

        return RedirectToAction("Index", new { tab = "topic" });
    }
}