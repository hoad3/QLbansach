using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services
{
    public interface IAdminService
    {
        // Book management
        Task<List<Sach>> GetAllBooksAsync();
        Task<Sach?> GetBookByIdAsync(int id);
        Task<(bool success, string message)> CreateBookAsync(Sach book);
        Task<(bool success, string message)> UpdateBookAsync(Sach book);
        Task<(bool success, string message)> DeleteBookAsync(int id);

        // User management
        Task<List<Khachhang>> GetAllUsersAsync();
        Task<Khachhang?> GetUserByIdAsync(int id);
        Task<(bool success, string message)> UpdateUserAsync(Khachhang user);
        Task<(bool success, string message)> DeleteUserAsync(int id);
    }
} 