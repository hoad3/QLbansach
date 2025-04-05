using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public interface ISachService
{
    Task<List<Sach>> GetAllSachesAsync();
}