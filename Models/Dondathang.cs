using System;
using System.Collections.Generic;

namespace QLbansach_tutorial.Models;

public partial class Dondathang
{
    public int MaDonHang { get; set; }

    public DateOnly? Ngatdat { get; set; }

    public DateOnly? Ngaygiao { get; set; }

    public string? Tinhtranggiaohang { get; set; }

    public int? MaKh { get; set; }

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual Khachhang? MaKhNavigation { get; set; }
}
