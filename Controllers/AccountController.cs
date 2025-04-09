using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Models;
using QLbansach_tutorial.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace QLbansach_tutorial.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Auth(bool register = false)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        
        ViewBag.ShowRegister = register;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ShowRegister = false;
            return View("Auth");
        }
        var (success, message, user) = await _authService.LoginAsync(model);
        
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.ShowRegister = false;
            return View("Auth");
        }
        
        TempData["SuccessMessage"] = "Đăng nhập thành công!";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ShowRegister = true;
                return View("Auth");
            }

            if (await _authService.EmailExistsAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng");
                ViewBag.ShowRegister = true;
                return View("Auth");
            }

            if (await _authService.UsernameExistsAsync(model.Taikhoan))
            {
                ModelState.AddModelError("Taikhoan", "Tên tài khoản này đã được sử dụng");
                ViewBag.ShowRegister = true;
                return View("Auth");
            }
            bool result = await _authService.RegisterAsync(model);

            if (!result)
            {
                ModelState.AddModelError("", "Đăng ký không thành công, vui lòng thử lại");
                ViewBag.ShowRegister = true;
                return View("Auth");
            }
            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Bạn có thể đăng nhập ngay bây giờ.";
            return RedirectToAction("Auth");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
            ViewBag.ShowRegister = true;
            return View("Auth");
        }
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
} 