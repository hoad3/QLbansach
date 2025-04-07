using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Models;
using QLbansach_tutorial.Services;

namespace QLbansach_tutorial.Controllers;

public class SachController : Controller
{
    private readonly ISachService _sachService;
    private readonly IChudeService _chudeService;
    private readonly INhaxuatbanService _nxbService;
    private const int PageSize = 8;

    public SachController(ISachService sachService, IChudeService chudeService, INhaxuatbanService nxbService)
    {
        _sachService = sachService;
        _chudeService = chudeService;
        _nxbService = nxbService;
    }

    public async Task<IActionResult> Index(string? Search, int MaCD = 0, int MaNXB = 0, int Trang = 1)
    {
        var model = new PayloadIndex
        {
            Search = Search,
            MaCD = MaCD,
            MaNXB = MaNXB,
            Trang = Trang
        };
        
        var (saches, totalCount) = await _sachService.SearchSachesAsync(model);
        var chudes = await _chudeService.GetChude();
        var nhaxuatbans = await _nxbService.GetNhaxuatbansAsync();

        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        ViewBag.ChuDes = chudes;
        ViewBag.NhaXuatBans = nhaxuatbans;
        ViewBag.CurrentSearch = model;

        // Lấy tên chủ đề hoặc nhà xuất bản đang được chọn
        if (MaCD > 0)
        {
            var selectedChude = chudes.FirstOrDefault(c => c.MaCd == MaCD);
            ViewBag.SelectedCategory = selectedChude?.TenChuDe;
        }
        else if (MaNXB > 0)
        {
            var selectedNxb = nhaxuatbans.FirstOrDefault(n => n.MaNxb == MaNXB);
            ViewBag.SelectedCategory = selectedNxb?.TenNxb;
        }

        return View(saches);
    }

    [HttpGet]
    public async Task<IActionResult> LoadMore(string? Search, int MaCD = 0, int MaNXB = 0, int Trang = 1)
    {
        var model = new PayloadIndex
        {
            Search = Search,
            MaCD = MaCD,
            MaNXB = MaNXB,
            Trang = Trang
        };
        
        var (saches, _) = await _sachService.SearchSachesAsync(model);
        return PartialView("_BookList", saches);
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var sach = await _sachService.GetSachByIdAsync(id);
        if (sach == null)
        {
            return NotFound();
        }

        // Lấy sách liên quan (cùng chủ đề hoặc nhà xuất bản)
        var relatedBooks = await _sachService.GetRelatedSachesAsync(
            sach.MaCd ?? 0, 
            sach.MaNxb ?? 0, 
            sach.Masach);

        ViewBag.RelatedBooks = relatedBooks;
        
        return View(sach);
    }
} 