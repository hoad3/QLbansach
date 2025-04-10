using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using QLbansach_tutorial.Helpers;
using BCrypt.Net;

namespace QLbansach_tutorial.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Khachhangs.AnyAsync(x => x.Email == email);
    }
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Khachhangs.AnyAsync(x => x.Taikhoan == username);
    }
    public async Task<(bool success, string message, Khachhang? user)> LoginAsync(Login model)
    {
        try 
        {
            // Tìm user trong database và include IdQuyenNavigation
            var user = await _context.Khachhangs
                .Include(x => x.IdQuyenNavigation)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                return (false, "Email không tồn tại", null);
            }

            string hashedPassword = Helper.hashpassword(model.Password);

            if (user.Matkhau != hashedPassword)
            {
                return (false, "Mật khẩu không chính xác", null);
            }

            // Kiểm tra null cho IdQuyenNavigation
            string role = "User"; // Mặc định là User nếu không có quyền
            if (user.IdQuyenNavigation != null)
            {
                role = user.IdQuyenNavigation.TenQuyen ?? "User";
            }

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.HoTen ?? "Unknown"),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.MobilePhone, user.DienthoaiKh ?? ""),
                new Claim(ClaimTypes.StreetAddress, user.DiachiKh ?? ""),
                new Claim("AccountId", user.MaKh.ToString())
            };

            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return (true, "Đăng nhập thành công", user);
        }
        catch (Exception ex)
        {
            // Log lỗi nếu cần
            return (false, $"Lỗi hệ thống: {ex.Message}", null);
        }
    }

    public async Task<bool> RegisterAsync(Register model)
    {
        try
        {
            // Kiểm tra email đã tồn tại chưa
            if (await EmailExistsAsync(model.Email))
            {
                return false;
            }

            // Kiểm tra tên tài khoản đã tồn tại chưa
            if (await UsernameExistsAsync(model.Taikhoan))
            {
                return false;
            }

            string hashedPassword = Helper.hashpassword(model.Password);

            var khachhang = new Khachhang
            {
                HoTen = model.HoTen,
                Taikhoan = model.Taikhoan,
                Matkhau = hashedPassword,
                Email = model.Email,
                DiachiKh = model.DiachiKH,
                DienthoaiKh = model.DienthoaiKH,
                NgaySinh = new DateOnly(model.Nam, model.Thang, model.Ngay),
                IdQuyen = 2 
            };

            // Thêm vào database
            await _context.Khachhangs.AddAsync(khachhang);
            await _context.SaveChangesAsync();
            

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}