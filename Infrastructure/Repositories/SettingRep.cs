using Domain.Entitites;
using Infrastructure.Context;
using System.Data.Entity;
namespace Infrastructure.Repositories;

public interface ISettingRep
{
    Task<IEnumerable<Settings>> GetAllAsync();
}
public class SettingRep : ISettingRep
{
    private readonly ApplicationContext _context;
    public SettingRep(ApplicationContext context)
    {
        _context = context;
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
}
