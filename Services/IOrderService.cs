using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services
{
    public interface IOrderService
    {
        Task<(bool success, string message, int? orderId)> PlaceOrderAsync(int userId);
        Task<List<Dondathang>> GetOrderHistoryAsync(int userId);
        Task<Dondathang?> GetOrderDetailAsync(int orderId, int userId);
    }
} 