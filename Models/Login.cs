using System.ComponentModel.DataAnnotations;

namespace QLbansach_tutorial.Models;

public class Login
{
    [Required(ErrorMessage = "Tài khoản không được bỏ trống")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
    [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 3 ký tự", MinimumLength = 3)]
    public string Password { get; set; } = null!;
}