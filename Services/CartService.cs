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
            var existingItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.MaKh == userId && c.MaSach == bookId);

            if (existingItem != null)
            {
                existingItem.Sl += quantity;
            }
            else
            {
                var newItem = new Cart
                {
                    MaKh = userId,
                    MaSach = bookId,
                    Sl = quantity
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
                .FirstOrDefaultAsync(c => c.MaKh == userId && c.MaSach == bookId);

            if (item != null)
            {
                item.Sl = quantity;
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
        var total = await _context.Carts
            .Where(c => c.MaKh == userId)
            .SumAsync(c => c.Tongtien ?? 0);
        return total;
    }
} 
