using Application.Dtos.Round;
using Application.Services.Identity;
using Domain.Entitites;
using Infrastructure.Repositories.BaseRepository;

namespace Application.Services;
public interface IRoundService
{
    Task<bool> AddRound(RoundDto dto);
    Task<IEnumerable<RoundGet>> GetRounds();
    Task<bool> PassRound(RoundDto dto);
}

public class RoundService : IRoundService
{
    private readonly IBaseRepository<Round> roundRepo;
    private readonly IIdentityService userRep;

    public RoundService(IBaseRepository<Round> roundRepo, IIdentityService userRep)
    {
        this.roundRepo = roundRepo;
        this.userRep = userRep;
    }

    public async Task<bool> AddRound(RoundDto dto)
    {
        var round = new Round()
        {
            Id = dto.Id,
            CurrentRound = 1,
            Deadline = dto.Deadline
        };

        await roundRepo.AddAsync(round);

        return true;
    }

    public async Task<bool> PassRound(RoundDto dto)
    {
        var currentRound = await roundRepo.GetAsync(dto.Id);

        if(currentRound.CurrentRound < 4)
        {
            currentRound.Deadline = dto.Deadline;
            currentRound.CurrentRound++;

            await roundRepo.UpdateAsync(currentRound);

            return true;
        }

        return false;
    }

    public async Task<IEnumerable<RoundGet>> GetRounds()
    {
        var rounds = await roundRepo.GetAllAsync();
        return rounds.Select(round => new RoundGet
        {
            Id = round.Id,
            Deadline = round.Deadline
        });
    }
}
