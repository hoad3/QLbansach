using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Helpers;
using QLbansach_tutorial.Models;
using QLbansach_tutorial.Services;
using System.Linq;

namespace QLbansach_tutorial.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Users()
    {
        // Lấy danh sách người dùng
        var users = _context.Khachhangs.ToList();
        return View(users);
    }
    
    public IActionResult Products()
    {
        var products = _context.Saches.ToList();
        return View(products);
    }
    
    public IActionResult Orders()
    {
        var orders = _context.Dondathangs.ToList();
        return View(orders);
    }

    // [AllowAnonymous] 
    // public async Task<IActionResult> SeedData()
    // {
    //     try
    //     {
    //         if (!_context.Quyens.Any())
    //         {
    //             _context.Roles.Add(new Quyen { IdQuyen = 1, TenQuyen = "Admin" });
    //             
    //             _context.Quyens.Add(new Quyen { IdQuyen = 2, TenQuyen = "User" });
    //             
    //             await _context.SaveChangesAsync();
    //         }
    //         
    //         if (!_context.Khachhangs.Any(x => x.IdQuyen == 1))
    //         {
    //             _context.Khachhangs.Add(new Khachhang
    //             {
    //                 HoTen = "Administrator",
    //                 Taikhoan = "admin",
    //                 Matkhau = Helper.hashpassword("Admin@123"), 
    //                 Email = "admin@example.com",
    //                 IdQuyen = 1 
    //             });
    //             
    //             await _context.SaveChangesAsync();
    //         }
    //         
    //         return Content("Dữ liệu khởi tạo thành công!");
    //     }
    //     catch (Exception ex)
    //     {
    //         return Content($"Lỗi khi khởi tạo dữ liệu: {ex.Message}");
    //     }
    // }
} 