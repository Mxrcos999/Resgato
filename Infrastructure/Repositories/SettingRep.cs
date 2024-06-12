using Domain.Entitites;
using Infrastructure.Context;
using System.Data.Entity;
using System.Security.Claims;
namespace Infrastructure.Repositories;

public interface ISettingRep
{
    Task<IEnumerable<Settings>> GetAllAsync();
    Task<IEnumerable<Settings>> GetAllByIdAsync(int gameId);
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
                     .Include(x => x.Game)
                     .Include(x => x.SettingCat)
                     select new Settings()
                     {
                         ApplicationUser = setting.ApplicationUser,
                         Id = setting.Id,
                         Game = setting.Game,
                         ApplicationUserId = setting.ApplicationUserId, 
                         BudgetGame = setting.BudgetGame,
                         SettingCat = setting.SettingCat,
                                                  
                     };

        return result;
    }

    public async Task<IEnumerable<Settings>> GetAllByIdAsync(int gameId)
    {
        var result = from Settings setting in _context.Settings
                     .Include(x => x.ApplicationUser)
                     .Include(x => x.Game)
                     .Include(x => x.SettingCat)
                     .Where(x => x.ApplicationUser.Id == _userId && x.GameId == gameId)
                     select new Settings()
                     {
                         ApplicationUser = setting.ApplicationUser,
                         Id = setting.Id,
                         ApplicationUserId=setting.ApplicationUserId,
                         BudgetGame = setting.BudgetGame,
                         Game = setting.Game,
                         SettingCat = setting.SettingCat,
                         
                     };

        return result;
    }
}
