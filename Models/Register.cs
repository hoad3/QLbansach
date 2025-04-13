using System.ComponentModel.DataAnnotations;

namespace QLbansach_tutorial.Models;

public class Register
{
    [Required(ErrorMessage = "Họ tên không được bỏ trống")]
    public string HoTen { get; set; } = null!;
    
    [Required (ErrorMessage="Tên tài khoản không được bỏ trống")]
    public string Taikhoan { get; set; } = null!;
    
    [Required (ErrorMessage="Email không được bỏ trống")]
    [EmailAddress (ErrorMessage = "Địa chỉ Email không hợp lệ")]
    public string Email { get; set; } = null!;
    
    [Required (ErrorMessage="Mật khẩu không được bỏ trống")]
    [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 3 ký tự", MinimumLength = 3)]
    public string Password { get; set; } = null!;
    
    [Required (ErrorMessage="Vui lòng xác nhận lại mật khẩu")]
    [Compare("Password", ErrorMessage = "Mật khẩu và phần xác nhận không khớp với nhau")]
    public string RepeatPassword { get; set; } = null!;
    
    public string? DiachiKH { get; set; }
    public string? DienthoaiKH { get; set; }
    
    [Required(ErrorMessage = "Vui lòng chọn ngày")]
    public int Ngay { get; set; }
    
    [Required(ErrorMessage = "Vui lòng chọn tháng")]
    public int Thang { get; set; }
    
    [Required(ErrorMessage = "Vui lòng chọn năm")]
    public int Nam { get; set; }
    
    public DateOnly Ngaysinh { get; set; }
}