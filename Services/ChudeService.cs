using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public class ChudeService: IChudeService
{
    private readonly AppDbContext _context;

    public ChudeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Chude>> GetChude()
    {
        return await _context.Chudes.ToListAsync();
    }
}