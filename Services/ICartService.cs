using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public interface ICartService
{
    Task<IEnumerable<Cart>> GetCartItemsAsync(int userId);
    Task<decimal> CalculateTotal(int userId);
    Task<bool> AddToCartAsync(int userId, int bookId, int quantity = 1);
    Task<bool> UpdateCartItemAsync(int userId, int bookId, int quantity);
    Task<bool> RemoveFromCartAsync(int userId, int bookId);
    Task<bool> ClearCartAsync(int userId);
} 