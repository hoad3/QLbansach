using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Models;
using System.Security.Claims;
using QLbansach_tutorial.Services;

namespace QLbansach_tutorial.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IVnPayService _vnPayService;

    public OrderController(IOrderService orderService, ICartService cartService, IVnPayService vnPayService)
    {
        _vnPayService = vnPayService;
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
    
    [HttpGet]
    public async Task<IActionResult> PaymentCallbackVnpay()
    {
        var response = _vnPayService.PaymentExecute(Request.Query);
        
        if (response != null && response.VnPayResponseCode == "00")
        {
            // Thanh toán thành công, lưu thông tin đơn hàng
            var userId = int.Parse(User.FindFirstValue("AccountId"));
            var (success, message, orderId) = await _orderService.PlaceOrderAsync(userId);

            if (success)
            {
                TempData["PaymentSuccess"] = true;
                TempData["OrderId"] = orderId;
                // TempData["Amount"] = response.Amount / 100; // Chuyển về đơn vị tiền tệ
                TempData["PaymentDate"] = DateTime.Now;
                TempData["TransactionId"] = response.TransactionId;
                
                return RedirectToAction("PaymentSuccess");
            }
        }

        // Thanh toán thất bại hoặc có lỗi
        // TempData["PaymentError"] = response?.Message ?? "Có lỗi xảy ra trong quá trình thanh toán";
        return RedirectToAction("PaymentError");
    }

    public IActionResult PaymentSuccess()
    {
        if (TempData["PaymentSuccess"] == null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    public IActionResult PaymentError()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateVnPayPayment(PaymentInformationModel model)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập để thanh toán" });
        }

        try
        {
            if (model.Amount <= 0)
            {
                TempData["Error"] = "Số tiền thanh toán không hợp lệ";
                return RedirectToAction("Checkout");
            }
            if (string.IsNullOrEmpty(model.OrderType) || string.IsNullOrEmpty(model.OrderDescription))
            {
                TempData["Error"] = "Thiếu thông tin thanh toán";
                return RedirectToAction("Checkout");
            }

            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Redirect(paymentUrl);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Có lỗi xảy ra khi tạo URL thanh toán";
            return RedirectToAction("Checkout");
        }
    }
} 