using System;
using System.Collections.Generic;

namespace QLbansach_tutorial.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? TenQuyen { get; set; }

    public virtual ICollection<Khachhang> Khachhangs { get; set; } = new List<Khachhang>();
}
