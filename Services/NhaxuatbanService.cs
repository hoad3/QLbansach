using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public class NhaxuatbanService: INhaxuatbanService
{
    private readonly AppDbContext _context;

    public NhaxuatbanService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Nhaxuatban>> GetNhaxuatbansAsync()
    {
        return await _context.Nhaxuatbans.ToListAsync();
    }
}