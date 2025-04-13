using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public interface ISachService
{
    Task<List<Sach>> GetAllSachesAsync();
    Task<List<Sach>> GetFirst4SachesAsync();
    Task<List<Sach>> GetSachesByPageAsync(int page, int pageSize);
    Task<int> GetTotalSachesCountAsync();
    Task<(List<Sach> Saches, int TotalCount)> SearchSachesAsync(PayloadIndex model, int pageSize = 12);
    Task<Sach> GetSachByIdAsync(int id);
    Task<List<Sach>> GetRelatedSachesAsync(int maCD, int maNXB, int currentSachId, int limit = 4);
}