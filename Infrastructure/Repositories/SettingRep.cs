using Domain.Entitites;
using Infrastructure.Context;
using System.Data.Entity;
using System.Security.Claims;
namespace Infrastructure.Repositories;

public interface ISettingRep
{
    Task<IEnumerable<Settings>> GetAllAsync();
    Task<IEnumerable<Settings>> GetAllByIdAsync();
}
public class SettingRep : ISettingRep
{
    private readonly ApplicationContext _context;
    private readonly string _userId;

    public SettingRep(ApplicationContext context)
    {
        _context = context;
        _userId = _context._contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    }

    public async Task<IEnumerable<Settings>> GetAllAsync()
    {
        var result = from Settings setting in _context.Settings
                     .Include(x => x.ApplicationUser)
                     select new Settings()
                     {
                         ApplicationUser = setting.ApplicationUser,
                         Id = setting.Id,
                         CatsQuantity = setting.CatsQuantity,
                         Gender = setting.Gender
                         
                     };

        return result;
    }

    public async Task<IEnumerable<Settings>> GetAllByIdAsync()
    {
        var result = from Settings setting in _context.Settings
                     .Include(x => x.ApplicationUser)
                     .Where(x => x.ApplicationUser.Id == _userId)
                     select new Settings()
                     {
                         ApplicationUser = setting.ApplicationUser,
                         Id = setting.Id,
                         CatsQuantity = setting.CatsQuantity,
                         Gender = setting.Gender
                         
                     };

        return result;
    }
}
