using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Models.GroupViewModels;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup;
using PRN222_SWP_TOOL_MVC.Service.Services.StudentGroupService;
using System.Security.Claims;

namespace PRN222_SWP_TOOL_MVC.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IStudentGroupService _groupService;

        public GroupController(IStudentGroupService groupService)
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

        // GET: /Group/Create?classId=1
        [HttpGet]
        public IActionResult Create(int classId)
        {
            return View(new CreateGroupViewModel { ClassID = classId });
        }

        // POST: /Group/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = GetCurrentUserId();

            var dto = new CreateGroupRequestDTO
            {
                ClassID = vm.ClassID,
                GroupName = vm.GroupName
            };

            var result = await _groupService.CreateGroupAsync(dto, userId);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ResponseMessage);
                return View(vm);
            }

            return RedirectToAction("Details", new { groupId = result.Data });
        }

     
        [HttpGet]
        public async Task<IActionResult> Details(int groupId)
        {
            var result = await _groupService.GetGroupInfoAsync(groupId);
            if (!result.Success || result.Data == null)
            {
                var emptyVm = new GroupDetailsViewModel
                {
                    GroupID = 0,
                    GroupName = "(Chưa có nhóm)",
                    IsLocked = false,
                    Status = "EMPTY",

                    ClassID = 0,      // nếu bạn biết classId thì set vào đây
                    MaxMember = 6,    // bạn có thể set cứng
                    InviteCode = "",

                    MemberCount = 0,
                    Members = new List<GroupMemberItemViewModel>()
                };

               
                ViewBag.Message = result.ResponseMessage ?? "Bạn chưa có nhóm.";
                return View(emptyVm);
            }

            var dto = result.Data;

            var vm = new GroupDetailsViewModel
            {
                GroupID = dto.GroupID,
                GroupName = dto.GroupName,
                IsLocked = dto.IsLocked,
                Status = dto.Status,

                ClassID = dto.ClassID,
                MaxMember = dto.MaxMember,
                InviteCode = dto.InviteCode,

                MemberCount = dto.MemberCount,
                Members = dto.Members.Select(m => new GroupMemberItemViewModel
                {
                    StudentID = m.StudentID,
                    FullName = m.FullName,
                    IsLeader = m.IsLeader
                }).ToList()
            };

            return View(vm);
        }
    }
}