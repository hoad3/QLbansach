using System;
using System.Collections.Generic;

namespace QLbansach_tutorial.Models;

public partial class Chude
{
    public int MaCd { get; set; }

    public string? TenChuDe { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
