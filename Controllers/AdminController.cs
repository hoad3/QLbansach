using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Models;
using QLbansach_tutorial.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace QLbansach_tutorial.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly AppDbContext _context;

    public AdminController(IAdminService adminService, AppDbContext context)
    {
        _adminService = adminService;
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var orders = await _context.Dondathangs
            .Include(o => o.Chitietdonhangs)
                .ThenInclude(c => c.MasachNavigation)
            .Include(o => o.MaKhNavigation)
            .ToListAsync();
        return View(orders);
    }
    
    public IActionResult Users()
    {
        var users = _context.Khachhangs
            .Include(u => u.IdQuyenNavigation)
            .ToList();
        ViewBag.Roles = _context.Roles.ToList();
        return View(users);
    }
    
    public IActionResult Products()
    {
        var products = _context.Saches
            .Include(p => p.MaNxbNavigation)
            .Include(p => p.MaCdNavigation)
            .ToList();
        ViewBag.Publishers = _context.Nhaxuatbans.ToList();
        ViewBag.Categories = _context.Chudes.ToList();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> CreateBook()
    {
        ViewBag.Publishers = await _context.Nhaxuatbans.ToListAsync();
        ViewBag.Categories = await _context.Chudes.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromForm] Sach book, IFormFile Anhbia)
    {
        try
        {
            if (Anhbia == null || Anhbia.Length == 0)
            {
                return Json(new { success = false, message = "Vui lòng chọn ảnh bìa" });
            }

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Anhbia.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client", "images", fileName);

            // Tạo thư mục nếu chưa tồn tại
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Anhbia.CopyToAsync(stream);
            }

            // Lưu đường dẫn vào database
            book.Anhbia = "/client/images/" + fileName;

            _context.Saches.Add(book);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Thêm sách thành công" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi khi thêm sách: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditBook(int id)
    {
        var book = await _adminService.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        ViewBag.Publishers = await _context.Nhaxuatbans.ToListAsync();
        ViewBag.Categories = await _context.Chudes.ToListAsync();
        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> EditBook(Sach book)
    {
        var (success, message) = await _adminService.UpdateBookAsync(book);
        if (success)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Products");
        }
        TempData["ErrorMessage"] = message;
        return View(book);
    }

    [HttpPost]
    public IActionResult DeleteBook(int id)
    {
        try
        {
            var book = _context.Saches.Find(id);
            if (book == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sách cần xóa" });
            }

            // Kiểm tra xem sách có trong đơn hàng nào không
            var hasOrders = _context.Chitietdonhangs.Any(ct => ct.Masach == id);
            if (hasOrders)
            {
                return Json(new { success = false, message = "Không thể xóa sách vì đã có trong đơn hàng" });
            }

            _context.Saches.Remove(book);
            _context.SaveChanges();
            return Json(new { success = true, message = "Xóa sách thành công" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi khi xóa sách: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditUser(int id)
    {
        var user = await _adminService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(Khachhang user)
    {
        var (success, message) = await _adminService.UpdateUserAsync(user);
        if (success)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Users");
        }
        TempData["ErrorMessage"] = message;
        return View(user);
    }
    [HttpPost]
    public IActionResult UpdateUserField(int id, string field, string value)
    {
        try
        {
            var user = _context.Khachhangs.Find(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            switch (field)
            {
                case "HoTen":
                    user.HoTen = value;
                    break;
                case "Email":
                    user.Email = value;
                    break;
                case "DienthoaiKh":
                    user.DienthoaiKh = value;
                    break;
                case "DiachiKh":
                    user.DiachiKh = value;
                    break;
                case "IdQuyen":
                    if (int.TryParse(value, out int roleId))
                    {
                        user.IdQuyen = roleId;
                    }
                    break;
                default:
                    return Json(new { success = false, message = "Trường không hợp lệ" });
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        try
        {
            var user = _context.Khachhangs.Find(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            _context.Khachhangs.Remove(user);
            _context.SaveChanges();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult UpdateBook(int id, Dictionary<string, string> updates)
    {
        try
        {
            var book = _context.Saches.Find(id);
            if (book == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sách" });
            }

            foreach (var update in updates)
            {
                switch (update.Key)
                {
                    case "Tensach":
                        book.Tensach = update.Value;
                        break;
                    case "Giaban":
                        if (decimal.TryParse(update.Value, out decimal price))
                        {
                            book.Giaban = price;
                        }
                        break;
                    case "Soluongton":
                        if (int.TryParse(update.Value, out int quantity))
                        {
                            book.Soluongton = quantity;
                        }
                        break;
                    case "Mota":
                        book.Mota = update.Value;
                        break;
                    case "MaNxb":
                        if (int.TryParse(update.Value, out int publisherId))
                        {
                            book.MaNxb = publisherId;
                        }
                        break;
                    case "MaCd":
                        if (int.TryParse(update.Value, out int categoryId))
                        {
                            book.MaCd = categoryId;
                        }
                        break;
                }
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
} 