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
    public async Task<IActionResult> PlaceOrder(string phone, string address)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập để đặt hàng" });
        }

        if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
        {
            return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin giao hàng" });
        }

        var userId = int.Parse(User.FindFirstValue("AccountId"));
        var (success, message, orderId) = await _orderService.PlaceOrderAsync(userId, phone, address);

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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateVnPayPayment(PaymentInformationModel model, string phone, string address)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập để thanh toán" });
        }

        if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
        {
            TempData["Error"] = "Vui lòng nhập đầy đủ thông tin giao hàng";
            return RedirectToAction("Checkout");
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

            // Lưu thông tin giao hàng vào TempData để sử dụng trong callback
            TempData["DeliveryPhone"] = phone;
            TempData["DeliveryAddress"] = address;

            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Redirect(paymentUrl);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Có lỗi xảy ra khi tạo URL thanh toán";
            return RedirectToAction("Checkout");
        }
    }

    [HttpGet]
    public async Task<IActionResult> PaymentCallbackVnpay()
    {
        var response = _vnPayService.PaymentExecute(Request.Query);
        
        if (response != null && response.VnPayResponseCode == "00")
        {
            // Lấy lại thông tin giao hàng từ TempData
            var phone = TempData["DeliveryPhone"]?.ToString();
            var address = TempData["DeliveryAddress"]?.ToString();

            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            {
                TempData["PaymentError"] = "Không tìm thấy thông tin giao hàng";
                return RedirectToAction("PaymentError");
            }

            var userId = int.Parse(User.FindFirstValue("AccountId"));
            var (success, message, orderId) = await _orderService.PlaceOrderAsync(userId, phone, address);

            if (success)
            {
                TempData["PaymentSuccess"] = true;
                TempData["OrderId"] = orderId;
                // TempData["Amount"] = response.Amount / 100;
                TempData["PaymentDate"] = DateTime.Now;
                TempData["TransactionId"] = response.TransactionId;
                
                return RedirectToAction("PaymentSuccess");
            }
        }

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
} 