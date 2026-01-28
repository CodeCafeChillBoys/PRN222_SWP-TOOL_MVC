using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GoogleResponse()
        {
            // 1️⃣ Lấy identity sau khi Google xác thực
            var authResult = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
                return RedirectToAction("Login");

            var principal = authResult.Principal;

            // 2️⃣ Lấy claims Google
            var dto = new LoginRequestDTO
            {
                Email = principal.FindFirstValue(ClaimTypes.Email),
                FullName = principal.FindFirstValue(ClaimTypes.Name),
                ProviderUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            if (string.IsNullOrWhiteSpace(dto.Email))
                return RedirectToAction("Login");

            // 3️⃣ Gọi service xử lý DB
            var user = await _googleAuthService.LoginWithGoogleAsync(dto);

            // 4️⃣ Sign in hệ thống (cookie)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            // 5️⃣ Login thành công
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

    }
}
