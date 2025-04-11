using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Helpers;
using QLbansach_tutorial.Models;
using QLbansach_tutorial.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        var orders = _context.Dondathangs
            .Include(o => o.Chitietdonhangs)
                .ThenInclude(c => c.MasachNavigation)
            .Include(o => o.MaKhNavigation)
            .ToList();
        return View(orders);
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

    
} 