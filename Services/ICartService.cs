using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public interface ICartService
{
    Task<Cart?> GetCartByUserId(int userId);
    Task<bool> AddToCart(int userId, int bookId, int quantity = 1);
    Task<bool> UpdateCartItemQuantity(int userId, int bookId, int quantity);
    Task<bool> RemoveFromCart(int userId, int bookId);
    Task<bool> ClearCart(int userId);
    Task<decimal> CalculateTotal(int userId);
} 