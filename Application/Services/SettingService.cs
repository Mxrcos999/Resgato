using Application.Dtos;
using Domain.Entitites;
using Infrastructure.Repositories.BaseRepository;

namespace Application.Services;

public interface ISettingService
{
    Task<bool> CreateSetting(SettingDto dto);
    Task<IEnumerable<SettingDto>> GetSetting();
}
public class SettingService : ISettingService 
{
    private readonly IBaseRepository<Settings> _settingsRepository;

    public SettingService(IBaseRepository<Settings> settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public async Task<bool> CreateSetting(SettingDto dto)
    {
        var setting = new Settings()
        {
            Id = dto.Id,
            CatsQuantity = dto.CatsQuantity,
            Gender = dto.Gender,
        };

        await _settingsRepository.AddAsync(setting);

        return true;
    }

    public async Task<IEnumerable<SettingDto>> GetSetting()
    {


        var result = from setting in await _settingsRepository.GetAllAsync()
                     select new SettingDto()
                     {
                         CatsQuantity = setting.CatsQuantity,
                         Gender = setting.Gender,
                         Id = setting.Id
                     };

        return result;
    }


}
