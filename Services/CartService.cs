using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cart>> GetCartItemsAsync(int userId)
    {
        return await _context.Carts
            .Include(c => c.MaSachNavigation)
            .Where(c => c.MaKh == userId)
            .ToListAsync();
    }
    
    public async Task<bool> AddToCartAsync(int userId, int bookId, int quantity = 1)
    {
        try
        {
            var book = await _context.Saches.FindAsync(bookId);
            if (book == null) return false;

            var existingItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.MaKh == userId && c.MaSach == bookId);

            decimal price = book.Giaban ?? 0m;
            decimal total = price * quantity;

            if (existingItem != null)
            {
                existingItem.Sl += quantity;
                existingItem.Gia = price;
                existingItem.Tongtien = existingItem.Sl * price;
            }
            else
            {
                var newItem = new Cart
                {
                    MaKh = userId,
                    MaSach = bookId,
                    Sl = quantity,
                    Gia = price,
                    Tongtien = total
                };
                _context.Carts.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateCartItemAsync(int userId, int bookId, int quantity)
    {
        try
        {
            var item = await _context.Carts
                .Include(c => c.MaSachNavigation)
                .FirstOrDefaultAsync(c => c.MaKh == userId && c.MaSach == bookId);

            if (item != null)
            {
                decimal price = item.MaSachNavigation.Giaban ?? 0m;
                item.Sl = quantity;
                item.Gia = price;
                item.Tongtien = quantity * price;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveFromCartAsync(int userId, int bookId)
    {
        try
        {
            var item = await _context.Carts
                .FirstOrDefaultAsync(c => c.MaKh == userId && c.MaSach == bookId);

            if (item != null)
            {
                _context.Carts.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ClearCartAsync(int userId)
    {
        try
        {
            var items = await _context.Carts
                .Where(c => c.MaKh == userId)
                .ToListAsync();

            _context.Carts.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<decimal> CalculateTotal(int userId)
    {
        var cartItems = await GetCartItemsAsync(userId);
        decimal total = 0m;
        
        foreach (var item in cartItems)
        {
            decimal price = item.MaSachNavigation?.Giaban ?? 0m;
            total += price * (item.Sl ?? 0m);
        }
        
        return total;
    }
} 
