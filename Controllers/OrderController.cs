using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Models;
using System.Security.Claims;
using QLbansach_tutorial.Services;

namespace QLbansach_tutorial.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public OrderController(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Checkout()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Auth", new { returnUrl = "/Order/Checkout" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var cartItems = await _cartService.GetCartItemsAsync(userId);

        if (!cartItems.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        ViewBag.Total = await _cartService.CalculateTotal(userId);

        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập để đặt hàng" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var (success, message, orderId) = await _orderService.PlaceOrderAsync(userId);

        return Json(new { success, message, orderId });
    }

    public async Task<IActionResult> OrderHistory()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Auth", new { returnUrl = "/Order/OrderHistory" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var orders = await _orderService.GetOrderHistoryAsync(userId);

        return View(orders);
    }

    public async Task<IActionResult> OrderDetail(int id)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Auth", new { returnUrl = $"/Order/OrderDetail/{id}" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var order = await _orderService.GetOrderDetailAsync(id, userId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }
} 