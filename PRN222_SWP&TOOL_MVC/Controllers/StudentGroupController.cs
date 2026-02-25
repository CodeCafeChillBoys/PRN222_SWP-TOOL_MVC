using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Models.GroupViewModels;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IClassRepository;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup;
using System.Security.Claims;

namespace PRN222_SWP_TOOL_MVC.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IStudentGroupService _groupService;
        private readonly IClassRepository    _classRepository;

        public GroupController(IStudentGroupService groupService, IClassRepository classRepository)
        {
            _groupService    = groupService;
            _classRepository = classRepository;
        }

        private int GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                throw new InvalidOperationException("User is not authenticated");
            return int.Parse(userIdStr);
        }

        // GET: /Group/Create?classId=1
        [HttpGet]
        public async Task<IActionResult> Create(int classId = 0)
        {
            // Load danh sách classes để student chọn (nếu classId chưa được truyền về)
            var classes = (await _classRepository.GetAllAsync()).ToList();
            ViewBag.Classes = classes;

            return View(new CreateGroupViewModel { ClassID = classId });
        }

        // POST: /Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGroupViewModel vm)
        {
            if (!ModelState.IsValid || vm.ClassID <= 0)
            {
                if (vm.ClassID <= 0)
                    ModelState.AddModelError("ClassID", "Vui lòng chọn lớp học.");
                ViewBag.Classes = (await _classRepository.GetAllAsync()).ToList();
                return View(vm);
            }

            var userId = GetCurrentUserId();

            var dto = new CreateGroupRequestDTO
            {
                ClassID   = vm.ClassID,
                GroupName = vm.GroupName,
                MaxMember = vm.MaxMember > 0 ? vm.MaxMember : 5
            };

            var result = await _groupService.CreateGroupAsync(dto, userId);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ResponseMessage);
                ViewBag.Classes = (await _classRepository.GetAllAsync()).ToList();
                return View(vm);
            }

            return RedirectToAction("Details", new { groupId = result.Data });
        }

        // GET: /Group/Join
        [HttpGet]
        public IActionResult Join() => View();

        // POST: /Group/Join
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(string inviteCode)
        {
            var userId = GetCurrentUserId();
            var result = await _groupService.JoinGroupAsync(inviteCode, userId);
            if (!result.Success)
            {
                ViewBag.Error = result.ResponseMessage;
                return View();
            }
            TempData["Success"] = result.ResponseMessage;
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
                    GroupID   = 0,
                    GroupName = "(Chưa có nhóm)",
                    IsLocked  = false,
                    Status    = "EMPTY",
                    ClassID   = 0,
                    MaxMember = 5,
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
                GroupID    = dto.GroupID,
                GroupName  = dto.GroupName,
                IsLocked   = dto.IsLocked,
                Status     = dto.Status,
                ClassID    = dto.ClassID,
                MaxMember  = dto.MaxMember,
                InviteCode = dto.InviteCode,
                MemberCount = dto.MemberCount,
                Members    = dto.Members.Select(m => new GroupMemberItemViewModel
                {
                    StudentID = m.StudentID,
                    FullName  = m.FullName,
                    IsLeader  = m.IsLeader
                }).ToList()
            };

            return View(vm);
        }
    }
}