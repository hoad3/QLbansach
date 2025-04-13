using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public interface INhaxuatbanService
{
    Task<List<Nhaxuatban>> GetNhaxuatbansAsync();
}