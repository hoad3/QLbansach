using System;
using System.Collections.Generic;

namespace QLbansach_tutorial.Models;

public partial class Chitietdonhang
{
    public int MaDonHang { get; set; }

    public int Masach { get; set; }

    public int? Soluong { get; set; }
    public string? sdt { get; set; }
    public string? Diachi { get; set; }
    public virtual Dondathang MaDonHangNavigation { get; set; } = null!;

    public virtual Sach MasachNavigation { get; set; } = null!;
}
