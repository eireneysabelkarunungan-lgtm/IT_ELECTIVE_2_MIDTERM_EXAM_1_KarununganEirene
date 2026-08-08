using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceMonitoringSystem.DTOs;
using VehicleServiceMonitoringSystem.Helpers;
using VehicleServiceMonitoringSystem.Models;
using VehicleServiceMonitoringSystem.Repositories;

namespace VehicleServiceMonitoringSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return RedirectToAction("Index", "ServiceJob");
            }

            return View(new RegisterDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDto dto)
        {
            if (_userRepository.UsernameExists(dto.Username))
            {
                ModelState.AddModelError(nameof(dto.Username), "This username is already taken.");
            }

            if (_userRepository.EmailExists(dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "An account with this email already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var user = new User
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                Username = dto.Username.Trim(),
                Password = PasswordHelper.Hash(dto.Password)
            };

            _userRepository.Add(user);

            TempData["SuccessMessage"] = "Registration successful. Please log in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return RedirectToAction("Index", "ServiceJob");
            }

            return View(new LoginDto { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var user = _userRepository.GetByUsername(dto.Username);

            if (user is null || !PasswordHelper.Verify(dto.Password, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(dto);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.GivenName, user.FullName),
                new(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = dto.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (!string.IsNullOrEmpty(dto.ReturnUrl) && Url.IsLocalUrl(dto.ReturnUrl))
            {
                return Redirect(dto.ReturnUrl);
            }

            return RedirectToAction("Index", "ServiceJob");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
