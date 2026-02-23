using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication;

namespace PRN222_MVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IGoogleAuthService _googleAuthService;

        public AccountController(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        [HttpGet]
        public IActionResult SelectRole()
        {

            // Sử dụng TempData để tránh người dùng vào trái phép
            if (TempData["Email"] == null)
                return RedirectToAction("Login");


            // Lấy dữ liệu Google đã xác thực:
            ViewBag.Email = TempData["Email"];
            ViewBag.Name = TempData["Name"];
            ViewBag.ProviderUserId = TempData["ProviderUserId"];

            // giữ cho gọi request tiếp theo
            TempData.Keep();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SelectRole(SelectRoleRequestDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var result = await _googleAuthService.CreateGoogleUserAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ResponseMessage);
                return View();
            }

            await SignInUser(result.Data);

            // Lấy lại ReturnUrl từ TempData sau khi tạo User thành công
            string returnUrl = TempData["ReturnUrl"] as string;

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            Console.WriteLine(result.Data.Role.RoleName);
            // Nếu không có trang chờ sẵn, điều hướng theo Role mặc định
            return result.Data.Role.RoleName switch
            {
                "Student" => RedirectToAction("Index", "Student"),
                "Teacher" => RedirectToAction("Index", "Teacher"),
                "Admin" => RedirectToAction("Index", "Admin"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
                return RedirectToAction("Login", "Account");

            var principal = authResult.Principal;

            var dto = new LoginRequestDTO
            {
                Email = principal.FindFirstValue(ClaimTypes.Email),
                FullName = principal.FindFirstValue(ClaimTypes.Name),
                ProviderUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            if (string.IsNullOrWhiteSpace(dto.Email))
                return RedirectToAction("Login", "Account");

            var user = await _googleAuthService.LoginWithGoogleAsync(dto);

            //CHƯA CÓ USER → CHỌN ROLE
            if (user == null)
            {
                TempData["Email"] = dto.Email;
                TempData["Name"] = dto.FullName;
                TempData["ProviderUserId"] = dto.ProviderUserId;
                TempData["ReturnUrl"] = returnUrl; // Lưu vào TempData cho SelectRole dùng

                return RedirectToAction("SelectRole", "Account");
            }

            await SignInUser(user);

            // Kiểm tra nếu có returnUrl thì quay lại ngay trang đó
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            Console.WriteLine(user.Role.RoleName);

            // Fallback theo ROLE nếu không có returnUrl
            return user.Role.RoleName switch
            {
                "Student" => RedirectToAction("Index", "Student"),
                "Teacher" => RedirectToAction("Index", "Teacher"),
                "Admin" => RedirectToAction("Index", "Admin"),
                _ => RedirectToAction("Index", "Home")
            };
        }


        // Redirect sang Google
        [HttpGet]
        public IActionResult GoogleLogin(string returnUrl = null)
        {
            // Tạo RedirectUri bao gồm cả tham số returnUrl để GoogleResponse có thể nhận lại được
            var redirectUrl = Url.Action("GoogleResponse", "Account", new { returnUrl });

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public IActionResult Login()
        {
            return View();
        }


        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role.RoleName)
    };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }

}