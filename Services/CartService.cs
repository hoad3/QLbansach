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

    public async Task<Cart?> GetCartByUserId(int userId)
    {
        return await _context.Carts
            .Include(c => c.MaSachNavigation)
            .FirstOrDefaultAsync(c => c.MaKh == userId);
    }

    public async Task<bool> AddToCart(int userId, int bookId, int quantity = 1)
    {
        try
        {
            // Kiểm tra sách có tồn tại không
            var sach = await _context.Saches.FindAsync(bookId);
            if (sach == null) return false;

            // Kiểm tra giỏ hàng đã tồn tại chưa
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.MaKh == userId && c.MaSach == bookId);

            if (existingCart != null)
            {
                // Nếu đã tồn tại, cập nhật số lượng và tổng tiền
                existingCart.Sl += quantity;
                existingCart.Tongtien = existingCart.Sl * sach.Giaban;
            }
            else
            {
                var cart = new Cart
                {
                    MaKh = userId,
                    MaSach = bookId,
                    Sl = quantity,
                    Gia = sach.Giaban,
                    Tongtien = quantity * sach.Giaban
                };
                _context.Carts.Add(cart);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCartItemQuantity(int userId, int bookId, int quantity)
    {
        try
        {
            var cart = await _context.Carts
                .Include(c => c.MaSachNavigation)
                .FirstOrDefaultAsync(c => c.MaKh == userId && c.MaSach == bookId);

            if (cart == null) return false;

            cart.Sl = quantity;
            cart.Tongtien = quantity * cart.MaSachNavigation.Giaban;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RemoveFromCart(int userId, int bookId)
    {
        try
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.MaKh == userId && c.MaSach == bookId);

            if (cart == null) return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ClearCart(int userId)
    {
        try
        {
            var cartItems = await _context.Carts
                .Where(c => c.MaKh == userId)
                .ToListAsync();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
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