using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(Register model);
    Task<(bool success, string message, Khachhang? user)> LoginAsync(Login model);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
}