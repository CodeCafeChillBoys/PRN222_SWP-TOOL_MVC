using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication;

namespace PRN222_MVC.Controllers
{
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
            if (TempData["Email"] == null)
                return RedirectToAction("Login");

            ViewBag.Email = TempData["Email"];
            ViewBag.Name = TempData["Name"];
            ViewBag.ProviderUserId = TempData["ProviderUserId"];

            TempData.Keep(); // ⚠️ rất quan trọng

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SelectRole(SelectRoleRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            var result = await _googleAuthService.CreateGoogleUserAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ResponseMessage);
                return View();
            }

            await SignInUser(result.Data);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            // 1️.Lấy identity sau khi Google xác thực
            var authResult = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
                return RedirectToAction("Login");

            var principal = authResult.Principal;

            // 2️.Lấy claims Google
            var dto = new LoginRequestDTO
            {
                Email = principal.FindFirstValue(ClaimTypes.Email),
                FullName = principal.FindFirstValue(ClaimTypes.Name),
                ProviderUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            if (string.IsNullOrWhiteSpace(dto.Email))
                return RedirectToAction("Login");

            // 3️. Gọi service xử lý DB
            var user = await _googleAuthService.LoginWithGoogleAsync(dto);

            // Chưa có User

            if (user == null)
            {
                TempData["Email"] = dto.Email;
                TempData["Name"] = dto.FullName;
                TempData["ProviderUserId"] = dto.ProviderUserId;

                return RedirectToAction("SelectRole");
            }

            // ĐÃ CÓ USER
            await SignInUser(user);

            // 5️. Login thành công
            return RedirectToAction("Index", "Home");
        }

        // Redirect sang Google
        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", "Account")
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
          new Claim("RoleID", user.RoleID.ToString())
    };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

    }
}
