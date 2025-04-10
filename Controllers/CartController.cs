using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly AppDbContext _context;

    public CartController(ICartService cartService, AppDbContext context)
    {
        _cartService = cartService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Auth", new { returnUrl = "/Cart" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var cartItems = await _context.Carts
            .Include(c => c.MaSachNavigation)
            .Where(c => c.MaKh == userId)
            .ToListAsync();

        ViewBag.Total = await _cartService.CalculateTotal(userId);
        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int bookId, int quantity = 1)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng" });
            }

            var userId = int.Parse(User.FindFirstValue("AccountId"));
            var success = await _cartService.AddToCart(userId, bookId, quantity);
            
            if (success)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Không thể thêm sản phẩm vào giỏ hàng" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int bookId, int quantity)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập để cập nhật giỏ hàng" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var success = await _cartService.UpdateCartItemQuantity(userId, bookId, quantity);
        if (success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = "Không thể cập nhật số lượng" });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItem(int bookId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập để xóa sản phẩm khỏi giỏ hàng" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var success = await _cartService.RemoveFromCart(userId, bookId);
        if (success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = "Không thể xóa sản phẩm khỏi giỏ hàng" });
    }

    [HttpPost]
    public async Task<IActionResult> ClearCart()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập để xóa giỏ hàng" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var success = await _cartService.ClearCart(userId);
        if (success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = "Không thể xóa giỏ hàng" });
    }

    public async Task<int> GetCartCount()
    {
        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var count = await _context.Carts
            .Where(c => c.MaKh == userId)
            .SumAsync(c => c.Sl ?? 0);
        return count;
    }
} 