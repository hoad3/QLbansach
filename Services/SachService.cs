using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public class SachService:ISachService
{
    private readonly AppDbContext _context;

    public SachService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sach>> GetAllSachesAsync()
    {
        return await _context.Saches.ToListAsync();
    }
}