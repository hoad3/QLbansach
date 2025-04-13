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

    public async Task<List<Sach>> GetFirst4SachesAsync()
    {
        return await _context.Saches.Take(4).ToListAsync();
    }

    public async Task<List<Sach>> GetSachesByPageAsync(int page, int pageSize)
    {
        return await _context.Saches
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalSachesCountAsync()
    {
        return await _context.Saches.CountAsync();
    }

    public async Task<(List<Sach> Saches, int TotalCount)> SearchSachesAsync(PayloadIndex model, int pageSize = 12)
    {
        string search = model.Search ?? "";
        
        var query = _context.Saches
            .Where(x => (x.Tensach!.Contains(search) || search == "")
                && ((x.MaCd == model.MaCD || model.MaCD == 0) 
                && (x.MaNxb == model.MaNXB || model.MaNXB == 0)));

        var totalCount = await query.CountAsync();
        
        var saches = await query
            .Skip((model.Trang - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (saches, totalCount);
    }
    
    public async Task<Sach?> GetSachByIdAsync(int id)
    {
        return await _context.Saches
            .Include(s => s.MaCdNavigation)
            .Include(s => s.MaNxbNavigation)
            .FirstOrDefaultAsync(s => s.Masach == id);
    }

    public async Task<List<Sach>> GetRelatedSachesAsync(int maCD, int maNXB, int currentSachId, int limit = 4)
    {
        return await _context.Saches
            .Where(s => (s.MaCd == maCD || s.MaNxb == maNXB) && s.Masach != currentSachId)
            .Take(limit)
            .ToListAsync();
    }
}