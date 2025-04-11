using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        // Book management
        public async Task<List<Sach>> GetAllBooksAsync()
        {
            return await _context.Saches
                .Include(s => s.MaNxbNavigation)
                .Include(s => s.MaCdNavigation)
                .ToListAsync();
        }

        public async Task<Sach?> GetBookByIdAsync(int id)
        {
            return await _context.Saches
                .Include(s => s.MaNxbNavigation)
                .Include(s => s.MaCdNavigation)
                .FirstOrDefaultAsync(s => s.Masach == id);
        }

        public async Task<(bool success, string message)> CreateBookAsync(Sach book)
        {
            try
            {
                _context.Saches.Add(book);
                await _context.SaveChangesAsync();
                return (true, "Thêm sách thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi thêm sách: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> UpdateBookAsync(Sach book)
        {
            try
            {
                var existingBook = await _context.Saches.FindAsync(book.Masach);
                if (existingBook == null)
                {
                    return (false, "Không tìm thấy sách");
                }

                _context.Entry(existingBook).CurrentValues.SetValues(book);
                await _context.SaveChangesAsync();
                return (true, "Cập nhật sách thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi cập nhật sách: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Saches.FindAsync(id);
                if (book == null)
                {
                    return (false, "Không tìm thấy sách");
                }

                _context.Saches.Remove(book);
                await _context.SaveChangesAsync();
                return (true, "Xóa sách thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi xóa sách: {ex.Message}");
            }
        }

        // User management
        public async Task<List<Khachhang>> GetAllUsersAsync()
        {
            return await _context.Khachhangs
                .Include(k => k.IdQuyenNavigation)
                .ToListAsync();
        }

        public async Task<Khachhang?> GetUserByIdAsync(int id)
        {
            return await _context.Khachhangs
                .Include(k => k.IdQuyenNavigation)
                .FirstOrDefaultAsync(k => k.MaKh == id);
        }

        public async Task<(bool success, string message)> UpdateUserAsync(Khachhang user)
        {
            try
            {
                var existingUser = await _context.Khachhangs.FindAsync(user.MaKh);
                if (existingUser == null)
                {
                    return (false, "Không tìm thấy người dùng");
                }

                // Không cập nhật mật khẩu
                user.Matkhau = existingUser.Matkhau;

                _context.Entry(existingUser).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
                return (true, "Cập nhật thông tin người dùng thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi cập nhật thông tin người dùng: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Khachhangs.FindAsync(id);
                if (user == null)
                {
                    return (false, "Không tìm thấy người dùng");
                }

                _context.Khachhangs.Remove(user);
                await _context.SaveChangesAsync();
                return (true, "Xóa người dùng thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi xóa người dùng: {ex.Message}");
            }
        }
    }
} 