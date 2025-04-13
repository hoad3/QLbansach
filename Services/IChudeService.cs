using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public interface IChudeService
{
    Task<List<Chude>> GetChude();
}