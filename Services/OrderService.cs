using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly ICartService _cartService;

        public OrderService(AppDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<(bool success, string message, int? orderId)> PlaceOrderAsync(int userId)
        {
            try
            {
                var cartItems = await _context.Carts
                    .Include(c => c.MaSachNavigation)
                    .Where(c => c.MaKh == userId)
                    .ToListAsync();

                if (!cartItems.Any())
                {
                    return (false, "Giỏ hàng trống", null);
                }

                // Tạo đơn hàng mới
                var order = new Dondathang
                {
                    MaKh = userId,
                    Ngatdat = DateOnly.FromDateTime(DateTime.Now),
                    Tinhtranggiaohang = "Đang xử lý"
                };

                _context.Dondathangs.Add(order);
                await _context.SaveChangesAsync();

                // Thêm chi tiết đơn hàng
                foreach (var item in cartItems)
                {
                    var orderDetail = new Chitietdonhang
                    {
                        MaDonHang = order.MaDonHang,
                        Masach = item.MaSach.Value,
                        Soluong = item.Sl
                    };

                    // Cập nhật số lượng tồn
                    var book = await _context.Saches.FindAsync(item.MaSach);
                    if (book != null)
                    {
                        book.Soluongton -= item.Sl;
                        if (book.Soluongton < 0)
                        {
                            throw new Exception($"Sản phẩm {book.Tensach} không đủ số lượng tồn");
                        }
                    }

                    _context.Chitietdonhangs.Add(orderDetail);
                }

                // Xóa giỏ hàng
                _context.Carts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                return (true, "Đặt hàng thành công", order.MaDonHang);
            }
            catch (Exception ex)
            {
                return (false, "Có lỗi xảy ra: " + ex.Message, null);
            }
        }

        public async Task<List<Dondathang>> GetOrderHistoryAsync(int userId)
        {
            return await _context.Dondathangs
                .Include(o => o.Chitietdonhangs)
                    .ThenInclude(od => od.MasachNavigation)
                .Where(o => o.MaKh == userId)
                .OrderByDescending(o => o.Ngatdat)
                .ToListAsync();
        }

        public async Task<Dondathang?> GetOrderDetailAsync(int orderId, int userId)
        {
            return await _context.Dondathangs
                .Include(o => o.Chitietdonhangs)
                    .ThenInclude(od => od.MasachNavigation)
                .FirstOrDefaultAsync(o => o.MaDonHang == orderId && o.MaKh == userId);
        }
    }
} 