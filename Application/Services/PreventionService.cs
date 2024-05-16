using Application.Dtos.Prevention;
using Domain.Entitites;
using Infrastructure.Repositories.BaseRepository;

namespace Application.Services;

public interface IPreventionService
{
    Task<bool> AddPrevention(PreventionDto dto);
    Task<IEnumerable<PreventionDto>> GetPrevention();
}

public class PreventionService : IPreventionService
{
    private readonly IBaseRepository<PreventionAction> preventionRepo;
    public PreventionService(IBaseRepository<PreventionAction> preventionRepo)
    {
        this.preventionRepo = preventionRepo;
    }

    public async Task<bool> AddPrevention(PreventionDto dto)
    {
        var prevention = new PreventionAction()
        {
            Action = dto.Action,
            Value = dto.Value,
            Gender = dto.Gender
        };

        await preventionRepo.AddAsync(prevention);

        return true;
    }

    public async Task<IEnumerable<PreventionDto>> GetPrevention()
    {
       return  (from prevention in await preventionRepo.GetAllAsync()
         select new PreventionDto()
         {
             Action = prevention.Action,
             Value = prevention.Value,
             Gender = prevention.Gender
         }).AsEnumerable();
    }
}
