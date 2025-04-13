using System;
using System.Collections.Generic;

namespace QLbansach_tutorial.Models;

public partial class Khachhang
{
    public int MaKh { get; set; }

    public string? HoTen { get; set; }

    public string? Taikhoan { get; set; }

    public string? Matkhau { get; set; }

    public string? Email { get; set; }

    public string? DiachiKh { get; set; }

    public string? DienthoaiKh { get; set; }

    public int? IdQuyen { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Dondathang> Dondathangs { get; set; } = new List<Dondathang>();

    public virtual Role? IdQuyenNavigation { get; set; }
}
